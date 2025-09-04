namespace Experimental.SourceGenerator;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var a = Guid.NewGuid();

        // ...
    }
}
