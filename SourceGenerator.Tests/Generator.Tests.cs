namespace Experimental.SourceGenerator.Tests;

[TestFixture]
public class Tests
{
    [Test, Category("Generator.Tests")]
    public void Compiles()
    {
        var generator = new SampleGenerator();

        var solutionDirectory = Directory
            .GetCurrentDirectory()
            .Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)
            .SkipLast(4)
            .Prepend(Path.DirectorySeparatorChar.ToString())
            .Aggregate(Path.Join);

        var projectDirectory = Path.Join(solutionDirectory, $"{nameof(Common)}");

        var syntaxTrees = Directory
            .EnumerateFiles(projectDirectory, "*.cs", SearchOption.AllDirectories)
            .Where(sourceFile => !sourceFile.StartsWith(Path.Join(projectDirectory, "obj")) && !sourceFile.EndsWith(".t.cs"))
            .Select(File.ReadAllText)
            .Select(source => CSharpSyntaxTree.ParseText(source));

        var compilation = CSharpCompilation
            .Create($"{nameof(Experimental)}.{nameof(Common)}")
            .AddReferences(MetadataReference.CreateFromFile(typeof(Model).Assembly.Location))
            .AddSyntaxTrees(syntaxTrees);

        //var compilation = CSharpCompilation.Create($"{nameof(Experimental)}.{nameof(SourceGenerator)}.{nameof(Experimental.SourceGenerator.Tests)}");

        CSharpGeneratorDriver
            .Create(generator)
            .RunGenerators(compilation);

        Assert.Pass();
    }
}
