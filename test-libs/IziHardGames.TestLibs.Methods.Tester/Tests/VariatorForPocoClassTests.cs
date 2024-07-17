using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace IziHardGames.Libs.Methods.Tester.Tests;

public class VariatorForPocoClassTests
{
    private readonly ITestOutputHelper console;

    private readonly JsonSerializerOptions opt = new JsonSerializerOptions()
    {
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        WriteIndented = true,
    };

    public VariatorForPocoClassTests(ITestOutputHelper helper)
    {
        this.console = helper;
    }

    [Fact]
    private void Test_GetVaritor()
    {
        var registry = new RegistryForVariators();
        var target = new VariatorForPocoClass();
        var variants = target.VariantsForPoco<DummyPocoVariatorForPocoClassTests>(registry);

        foreach (var variant in variants)
        {
            Assert.NotNull(variant);
            console.WriteLine(JsonSerializer.Serialize(variant, opt));
        }
    }

    internal class DummyPocoVariatorForPocoClassTests
    {
        public int PropInt { get; set; }
        public float PropFloat { get; set; }
        public double PropDuble { get; set; }
        public long PropLong { get; set; }
        public string? PropString { get; set; }
        public DateTime PropDateTime { get; set; }
    }
}