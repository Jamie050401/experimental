namespace Experimental.SourceGenerator.Common;

public class Template(AdditionalText template, CancellationToken cancellationToken)
{
    public string Content
    {
        get
        {
            if (_sourceText != null)
                return _sourceText.ToString();

            _sourceText = template.GetText(cancellationToken) ?? SourceText.From(string.Empty, Encoding.UTF8);
            return _sourceText.ToString();
        }
    }

    public string Name { get; } = Path.GetFileName(template.Path);

    private SourceText? _sourceText;
}
