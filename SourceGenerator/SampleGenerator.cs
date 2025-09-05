namespace Experimental.SourceGenerator;

[Generator]
public class SampleGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var templateProvider = context.GetTemplateProvider();

        var classProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node
            )
            .Where(cds => cds != null)
            .Collect();

        var sourceProvider = context.CompilationProvider
            .Combine(templateProvider)
            .Combine(classProvider)
            .Select((tuple, _) => (tuple.Left.Left, tuple.Left.Right, tuple.Right));

        context.RegisterSourceOutput(sourceProvider, Generate);
    }

    private static void Generate(SourceProductionContext context, (Compilation Left, ImmutableArray<Template>, ImmutableArray<ClassDeclarationSyntax>) sourceProvider)
    {
        var (compilation, templates, classes) = sourceProvider;

        if (templates.TryGetTemplate("Classes.t.cs", out var template))
        {
            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                "SG0001",
                "Missing required source generation template (Classes.t.cs).",
                string.Empty,
                string.Empty,
                DiagnosticSeverity.Error,
                true
            ), Location.None));

            return;
        }

        var stringBuilder = new StringBuilder();
        foreach (var @class in classes)
        {
            if (compilation.GetSemanticModel(@class.SyntaxTree).GetDeclaredSymbol(@class) is not INamedTypeSymbol symbol)
                continue;

            stringBuilder.Append($"\"{symbol.ToDisplayString()}\",\n");
            stringBuilder.Remove(stringBuilder.Length - 1, 3);
        }

        var source = template!.Content.Replace("${names}", stringBuilder.ToString());
        context.AddSource("Classes.g.cs", source);
    }
}
