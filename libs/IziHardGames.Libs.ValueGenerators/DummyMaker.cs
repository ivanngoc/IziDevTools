using System.Reflection;
using IziHardGames.Libs.Methods.Tester;
using IziHardGames.Libs.Methods.Tester.Randoms;
using IziHardGames.TestLibs.ValueGenerators.ForEfCore;

namespace IziHardGames.TestLibs.ValueGenerators;

public static class DummyMaker
{
    /// <summary>
    /// Получить граф объектов. Главным образом для EF Core. значения рандомные, не дефолтные. Уникальность гарантируется только для string свойств и полей
    /// </summary>
    /// <param name="opt"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static T GetDummyGraphOf<T>(DumyMakerOptions opt)
    {
        var root = GraphGenerator.GenerateBinaryGraphGen<T>(opt.DepthMax, opt);
        return root;
    }
}

public class DumyMakerOptions : IObjectGenerationOptions
{
    public BindingFlags BindingFlagsProperties { set; get; }
    public BindingFlags BindingFlagsFields { set; get; }

    /// <summary>
    /// Для объектов коллекций создавать 2 объекта
    /// </summary>
    public bool IsBinaryTreeMode { get; set; }

    public bool IsEfCoreSpecific { get; set; }
    public bool IsConsiderEfCoreNavigationProperty { get; set; }

    public int CountEnumerables { get; set; }
    public int DepthMax { get; set; }
    public bool IsUniqString { get; set; }
    public bool IsBinaryTree { get; }

    public BindingFlags FlagsForFields
    {
        get => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    }

    public BindingFlags FlagsForProperties
    {
        get => BindingFlags.Instance | BindingFlags.Public;
    }

    public bool CreateNullable { get => true; }

    public IGraphContext Context
    {
        get => context;
    }

    public bool IsIncludeProps { get => false; }

    public bool IsUniqGuid
    {
        get => true;
    }

    public bool IsAllowSelfNesting
    {
        get => false;
    }

    private EfObjectGeneratorGraphContext context = new EfObjectGeneratorGraphContext();
}