namespace Experimental.SourceGenerator;

public static class Helpers
{
    public static IncrementalValueProvider<ImmutableArray<(string Name, string Content)>> GetTemplateProvider(this IncrementalGeneratorInitializationContext context)
    {
        return context.AdditionalTextsProvider
            .Where(additionalText => additionalText.Path.EndsWith(".t.cs"))
            .Select((template, cancellationToken) => (Name: Path.GetFileName(template.Path), Content: template.GetText(cancellationToken)?.ToString()))
            .Where(template => template is { Name: not null, Content: not null })
            .Collect()!;
    }

    // ReSharper disable once InconsistentNaming
    public static bool Try<T>(Func<T> Function, out T? value)
    {
        value = default;

        try
        {
            value = Function();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
