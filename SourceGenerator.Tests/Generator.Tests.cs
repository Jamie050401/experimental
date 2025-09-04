namespace Experimental.SourceGenerator.Tests;

[TestFixture]
public class Tests
{
    [Test, Category("Generator.Tests")]
    public void Compiles()
    {
        var generator = new Generator();

        var solutionDirectory = Directory
            .GetCurrentDirectory()
            .Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)
            .SkipLast(4)
            .Prepend(Path.DirectorySeparatorChar.ToString())
            .Aggregate(Path.Join);

        var projectDirectory = Path.Join(solutionDirectory, $"{nameof(Common)}");

        var syntaxTrees = Directory
            .EnumerateFiles(projectDirectory, "*.cs", SearchOption.AllDirectories)
            .Where(sourceFile => !sourceFile.StartsWith(Path.Join(projectDirectory, "obj")))
            .Select(File.ReadAllText)
            .Select(source => CSharpSyntaxTree.ParseText(source));

        var compilation = CSharpCompilation
            .Create("Experimental.Common")
            .AddReferences(MetadataReference.CreateFromFile(typeof(Model).Assembly.Location))
            .AddSyntaxTrees(syntaxTrees);

        var driver = CSharpGeneratorDriver
            .Create(generator)
            .RunGenerators(compilation);

        driver.GetRunResult();

        Assert.Pass();
    }
}
