using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester;

public class ReflectionsHelperForFields
{
    public static bool IsNotAutoPropertyField(FieldInfo field)
    {
        return field.Name.EndsWith(">k__BackingField");
    }
    
    public static IEnumerable<FieldInfo> GetAllInstanceFields(Type type, BindingFlags flags)
    {
        foreach (var field in type.GetFields(flags))
        {
            yield return field;
        }

        if (type.BaseType != null && flags.HasFlag(BindingFlags.NonPublic))
        {
            foreach (var prop in GetAllPrivateFields(type.BaseType))
            {
                yield return prop;
            }
        }
    }

    private static IEnumerable<FieldInfo> GetAllPrivateFields(Type type)
    {
        foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
        {
            if ((field.IsPrivate) && (field.IsPrivate))
            {
                yield return field;
            }
        }

        if (type.BaseType != null)
        {
            foreach (var field in GetAllPrivateFields(type.BaseType))
            {
                yield return field;
            }
        }
    }
}