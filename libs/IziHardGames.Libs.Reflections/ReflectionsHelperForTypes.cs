using System.Collections;

namespace IziHardGames.Libs.Methods.Tester;

public class ReflectionsHelperForTypes
{
    public static bool IsEnumerable(Type type)
    {
        if (type == typeof(Array)) return true;
        var interfaces = type.GetInterfaces();
        if (interfaces.Any(x => x.IsAssignableTo(typeof(IEnumerable)))) return true;

        // foreach (var i in interfaces)
        // {
        //     if (i is ICollection) return true;
        //     if (i is IList) return true;
        //     if (i is IDictionary) return true;
        // }
        return false;
    }
}