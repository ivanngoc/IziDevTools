using System.Collections;
using System.Data.Entity.Spatial;
using System.Reflection;
using System.Text.Json;
using IziHardGames.Libs.Methods.Tester;
using IziHardGames.TestLibs.ValueGenerators;
using TestPackage;

namespace Play_With_Generators;

class Program
{
    static async Task Main(string[] args)
    {
        // Console.WriteLine("Hello, World!");
        // Console.WriteLine(typeof(IEnumerable<int>).IsAssignableTo(typeof(IEnumerable)));
        // return;

        var props = ReflectionsHelperForFields.GetAllInstanceFields(typeof(Class0), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToArray();

        var opt = new DumyMakerOptions()
        {
            // IsEfCoreSpecific = true,
            // IsConsiderEfCoreNavigationProperty = true,
            IsUniqString = true,
            CountEnumerables = 3,
            DepthMax = 10,
        };

        var dummy = DummyMaker.GetDummyGraphOf<MainModel>(opt);

        Console.WriteLine(JsonSerializer.Serialize(dummy, new JsonSerializerOptions()
        {
            WriteIndented = true
        }));

        if (false)
        {
            using var c = new DbSetTest();
            c.Add(dummy);
            await c.SaveChangesAsync();
        }

        var infos = opt.Context.DepthInfos;
        foreach (var info in infos)
        {
            Console.WriteLine($"{info.maxDepth}; {info.type.AssemblyQualifiedName}");
        }
    }
}

internal class Root
{
    public Nested1? Nested1AsProp { get; set; }
    public Nested1? Nested1AsField;
    public List<Nested1?> nested1AsCollection;
    public Nested2 nested2;
    public Nested3 nested3;
}

internal class Nested1
{
    public string? s0;
    public float f0;
    public float? fNull0;
}

internal struct Nested2
{
}

internal struct Nested3
{
    private Nested1? head;
    private Nested1? tail;
}

public class Class2
{
    private int PriateClass2 { get; set; }
    protected int ProtectedClass2 { get; set; }
    public int PublicClass2 { get; set; }
}

public class Class1 : Class2
{
    private int PriateClass1 { get; set; }
    protected int ProtectedClass1 { get; set; }
    public int PublicClass1 { get; set; }
}

public class Class0 : Class1
{
    private int PriateClass0 { get; set; }
    protected int ProtectedClass0 { get; set; }
    public int PublicClass0 { get; set; }
}