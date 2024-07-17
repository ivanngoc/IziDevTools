using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester;

public class ReflectionsHelperForNullable
{
    public static Nullable<T> WrapToNullableGen<T>(T result) where T : struct
    {
        return new System.Nullable<T>(result);
    }

    public static object WrapToNullable(Type type, object? value)
    {
        return ReflectionsHelperForMethods.MakeGenericCall<ReflectionsHelperForNullable>(nameof(WrapToNullableGen), BindingFlags.Public | BindingFlags.Static, type, null, value);
    }
}