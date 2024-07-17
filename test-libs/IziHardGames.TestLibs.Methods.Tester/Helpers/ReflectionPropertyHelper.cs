using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester;

public class ReflectionPropertyHelper
{
    public static bool IsNullable(PropertyInfo propertyInfo)
    {
        return Nullable.GetUnderlyingType(propertyInfo.PropertyType) == null;
    }
}