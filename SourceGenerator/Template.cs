namespace Experimental.SourceGenerator;

public class Template(string content, string name)
{
    public string Content { get; } = content;
    public string Name { get; } = name;
}
