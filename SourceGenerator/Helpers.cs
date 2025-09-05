namespace Experimental.SourceGenerator;

public static class Helpers
{
    public static IncrementalValueProvider<ImmutableArray<Template>> GetTemplateProvider(this IncrementalGeneratorInitializationContext context)
    {
        return context.AdditionalTextsProvider
            .Where(additionalText => additionalText.Path.EndsWith(".t.cs"))
            .Select((template, cancellationToken) =>
            {
                var content = template.GetText(cancellationToken)?.ToString();
                return content == null ? null : new Template(Path.GetFileName(template.Path), content);
            })
            .Where(template => template != null)
            .Collect()!;
    }

    public static bool TryGetTemplate(this ImmutableArray<Template> templates, string templateName, out Template? template)
    {
        template = null;

        try
        {
            template = templates.First(t => t.Name == templateName);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
