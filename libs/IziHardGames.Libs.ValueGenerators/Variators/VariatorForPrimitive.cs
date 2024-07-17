using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester;

public class VariatorForPrimitive : IVariator
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="type"></param>
    /// <returns>Func&lt;IEnumerable&lt;T&gt;&gt;<br/><see cref="Func{TResult}"/> as object where TResult is <see cref="IEnumerable{T}"/> where T is <see cref="type"/></returns>
    public static object CreateFuncAs(string methodName, Type type)
    {
        var mi = typeof(VariatorForPrimitive).GetMethod(nameof(CreateFuncAsGen), BindingFlags.Static | BindingFlags.Public)!;
        var gen = mi.MakeGenericMethod(type);
        return gen.Invoke(null, new object[] { methodName })!;
    }

    /// <summary>
    /// Get One of Variants methods as delegate T
    /// </summary>
    /// <param name="methodName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Func<IEnumerable<T>> CreateFuncAsGen<T>(string methodName)
    {
        return ReflectionsHelperForMethods.CreateFuncFromStaticAs<T>(typeof(VariatorForPrimitive), methodName);
    }

    public static IEnumerable<string?> VariantsForStrings()
    {
        string lipsum = " Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi semper massa eget faucibus vulputate. Proin auctor lorem eget ante pellentesque porttitor vitae eu turpis. Etiam porta justo quis ligula consectetur, non pellentesque metus lacinia. Praesent placerat sodales neque, nec fermentum dui fermentum et. Etiam egestas porta dictum. Duis ultricies varius lacus a efficitur. Morbi finibus orci nec commodo sagittis. ";
        return new string?[] { null, string.Empty, int.MinValue.ToString(), int.MaxValue.ToString(), float.MinValue.ToString(), float.MaxValue.ToString(), float.NaN.ToString(), 1f.ToString(), (-1f).ToString(), (-0.5f).ToString(), (0.5f).ToString(), (1 / 3f).ToString(), (decimal.One / 3).ToString(), decimal.MinValue.ToString(), decimal.MaxValue.ToString(), "short", "short string", DateTime.Now.ToString(CultureInfo.InvariantCulture), Guid.NewGuid().ToString(), typeof(VariatorForModel).AssemblyQualifiedName, lipsum };
    }

    public static IEnumerable<double> VariantsForDouble()
    {
        return new double[]
        {
            double.NegativeInfinity,
            double.MinValue,
            float.MinValue,
            -1,
            double.NegativeZero,
            double.NaN,
            float.NaN,
            default,
            double.Epsilon,
            1,
            double.Pi,
            float.MaxValue,
            double.MaxValue,
            double.PositiveInfinity
        };
    }

    public static IEnumerable<float> VariantsForFloat()
    {
        return new float[]
        {
            float.NegativeInfinity,
            float.NaN,
            float.MinValue,
            -1f,
            -0.5f,
            -1f / 3f,
            float.NegativeZero,
            0,
            float.Epsilon,
            0.5f,
            1f,
            1f / 3f,
            float.Pi,
            float.MaxValue,
            float.PositiveInfinity,
        };
    }

    public static IEnumerable<long> VariantsForLong()
    {
        return new long[]
        {
            long.MinValue,
            int.MinValue,
            -1,
            0,
            1,
            int.MaxValue,
            long.MaxValue
        };
    }

    public static IEnumerable<int> VariantsForInt() => new[]
    {
        int.MinValue,
        -1,
        0,
        1,
        int.MaxValue,
    };
}