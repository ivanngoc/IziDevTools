using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace IziHardGames.Libs.Methods.Tester;

public class VariatorForModelTests
{
    private readonly ITestOutputHelper console;
    private readonly JsonSerializerOptions opt = new JsonSerializerOptions()
    {
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        WriteIndented = true,
    };
    
    public VariatorForModelTests(ITestOutputHelper helper)
    {
        this.console = helper;
    }

    [Fact]
    private void Test_Variator()
    {
        var target = new VariatorForModel();
        var vars = target.GetTestingVariants<SomeClass>();

        foreach (var variant in vars)
        {
            Assert.NotNull(variant);
            console.WriteLine(JsonSerializer.Serialize(variant, opt));
        }
    }
}

