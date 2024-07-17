using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester;

public class ReflectionsHelperForMethods
{
    /// <summary>
    /// Search for Non-generic public & static Method that returns IEnumerable<T> and create Func<> from it 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="methodName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static Func<IEnumerable<T>> CreateFuncFromStaticAs<T>(Type type, string methodName)
    {
        var targetType = typeof(Func<IEnumerable<T>>);
        var mi = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public) ?? throw new NullReferenceException();
        var result = (Func<IEnumerable<T>>)Delegate.CreateDelegate(targetType, null, mi);
        return result;
    }

    public static bool IsSingleArgument(MethodInfo methodInfo)
    {
        return methodInfo.GetParameters().Length == 1;
    }

    public static object? MakeGenericCall<T>(string name, BindingFlags flags, Type typeParameter0, object? target, params object[] args)
    {
        var targetType = typeof(T);
        var mi = targetType.GetMethod(name, flags) ?? throw new NullReferenceException($"{name}. flags: {flags}");
        var gen = mi.MakeGenericMethod(typeParameter0);
        return gen.Invoke(target, args);
    }
}