using System;
using System.Collections.Generic;
using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester;

public class VariatorForBcl : IVariator
{
    public static IEnumerable<DateTime> VariantsForDateTime()
    {
        return new[]
        {
            default,
            DateTime.MinValue,
            DateTime.Now,
            DateTime.MaxValue,
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="type"></param>
    /// <returns>
    /// <returns>Func&lt;IEnumerable&lt;T&gt;&gt;<br/><see cref="Func{TResult}"/> as object where TResult is <see cref="IEnumerable{T}"/> where T is <see cref="type"/></returns>
    /// </returns>
    public static object CreateFuncAs(string methodName, Type type)
    {
        var mi = typeof(VariatorForBcl).GetMethod(nameof(CreateFuncAsGen), BindingFlags.Static | BindingFlags.Public)!;
        var gen = mi.MakeGenericMethod(type);
        return gen.Invoke(null, new object[] { methodName })!;
    }

    public static Func<IEnumerable<T>> CreateFuncAsGen<T>(string methodName)
    {
        return ReflectionsHelperForMethods.CreateFuncFromStaticAs<T>(typeof(VariatorForBcl), methodName);
    }
}