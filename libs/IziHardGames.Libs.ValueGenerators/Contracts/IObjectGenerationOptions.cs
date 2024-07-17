using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester;

public interface IObjectGenerationOptions
{
    /// <summary>
    /// если <see langword="true"/> то Reference-navigationProperty будет привязано к value-type Id полю если оно присутствует
    /// </summary>
    public bool IsConsiderEfCoreNavigationProperty { get; }

    /// <summary>
    /// Сколько создать объектов внутри  <see cref="IEnumerable{T}"/>
    /// </summary>
    public int CountEnumerables { get; }

    public bool IsUniqString { get; }
    public bool IsBinaryTree { get; }
    public BindingFlags FlagsForFields { get; }
    public BindingFlags FlagsForProperties { get; }

    public bool CreateNullable { get; }
    public IGraphContext Context { get; }
    public bool IsIncludeProps { get; }

    /// <summary>
    /// <see cref="Guid"/>
    /// </summary>
    public bool IsUniqGuid { get; }

    public bool IsAllowSelfNesting { get; }
}