namespace Experimental.SourceGenerator.Tests;

[TestFixture]
public class Tests
{
    [Test, Category("Generator.Tests")]
    public void Compiles()
    {
        var generator = new Generator();

        var compilation = CSharpCompilation
            .Create("Experimental.SourceGenerator.Tests.Assembly")
            .AddSyntaxTrees(CSharpSyntaxTree.ParseText(Source))
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver
            .Create(generator)
            .RunGenerators(compilation);

        driver.GetRunResult();

        Assert.Pass();
    }

    // ReSharper disable once UseRawString
    private const string Source = @"
namespace Experimental.SourceGenerator.Tests;

public class Program
{
    public static void Main(string[] args)
    {
        // ...
    }
}
";
}
