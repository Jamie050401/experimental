namespace Experimental.SourceGenerator.Common;

public static class Helpers
{
    public static IncrementalValueProvider<(TFirst First, TSecond Second, TThird Third)>  Combine<TFirst, TSecond, TThird>(
        this IncrementalValueProvider<TFirst> provider1,
        IncrementalValueProvider<TSecond> provider2,
        IncrementalValueProvider<TThird> provider3)
    {
        return provider1
            .Combine(provider2)
            .Combine(provider3)
            .Select((tuple, _) => (tuple.Left.Left, tuple.Left.Right, tuple.Right));
    }

    public static IncrementalValueProvider<ImmutableArray<Template>> GetTemplateProvider(this IncrementalGeneratorInitializationContext context)
    {
        return context.AdditionalTextsProvider
            .Where(additionalText => additionalText.Path.EndsWith(".t.cs"))
            .Select((template, cancellationToken) => new Template(template, cancellationToken))
            .Collect()!;
    }

    public static bool TryGetTemplate(this ImmutableArray<Template> templates, string templateName, out Template? template)
    {
        template = templates.FirstOrDefault(t => t.Name == templateName);
        return template != null;
    }
}
