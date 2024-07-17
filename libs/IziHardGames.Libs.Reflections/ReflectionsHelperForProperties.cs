using System.Collections;
using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester;

public class ReflectionsHelperForProperties
{
    public static bool IsInstanceGotBackingField(PropertyInfo propertyInfo)
    {
        Type type = propertyInfo.DeclaringType!;
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        return fields.Any(f => f.Name.Equals($"<{propertyInfo.Name}>k__BackingField"));
    }

    public static bool IsInstanceAutoProperty(PropertyInfo propertyInfo)
    {
        bool backingProp = IsInstanceGotBackingField(propertyInfo);
        return backingProp && propertyInfo.CanRead && propertyInfo.CanWrite;
    }

    /// <summary>
    /// Include private properties of all base classes if needed
    /// </summary>
    /// <param name="type"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetAllInstanceProperties(Type type, BindingFlags flags)
    {
        foreach (var prop in type.GetProperties(flags))
        {
            yield return prop;
        }

        if (type.BaseType != null && flags.HasFlag(BindingFlags.NonPublic))
        {
            foreach (var prop in GetAllPrivateProperties(type.BaseType))
            {
                yield return prop;
            }
        }
    }

    private static IEnumerable<PropertyInfo> GetAllPrivateProperties(Type type)
    {
        foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic))
        {
            var get = prop.GetMethod;
            var set = prop.SetMethod;

            if ((get?.IsPrivate ?? true) && (set?.IsPrivate ?? true))
            {
                yield return prop;
            }
        }

        if (type.BaseType != null)
        {
            foreach (var prop in GetAllPrivateProperties(type.BaseType))
            {
                yield return prop;
            }
        }
    }
}