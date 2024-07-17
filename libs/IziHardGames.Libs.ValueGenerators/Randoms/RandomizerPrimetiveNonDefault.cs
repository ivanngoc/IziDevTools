using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IziHardGames.Libs.Methods.Tester.Randoms;

public class RandomizerPrimetiveNonDefault : IRandomizer
{
    private readonly Dictionary<Type, object> funcsPerTypes = new Dictionary<Type, object>();

    public RandomizerPrimetiveNonDefault()
    {
        Func<CancellationToken, IEnumerable<int>> funcInt = RandomNonDefaultInt;
        Func<CancellationToken, IEnumerable<long>> funcLong = RandomNonDefaultLong;

        Func<CancellationToken, IEnumerable<float>> funcFloat = RandomNonDefaultFloat;
        Func<CancellationToken, IEnumerable<double>> funcDouble = RandomNonDefaultDouble;

        funcsPerTypes.Add(typeof(int), funcInt);
        funcsPerTypes.Add(typeof(long), funcLong);

        funcsPerTypes.Add(typeof(float), funcFloat);
        funcsPerTypes.Add(typeof(double), funcDouble);
    }

    private IEnumerable<double> RandomNonDefaultDouble(CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
        {
            var value = Random.Shared.NextDouble();
            if (value == default) continue;
            yield return value;
        }
    }

    private IEnumerable<long> RandomNonDefaultLong(CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
        {
            var value = Random.Shared.NextInt64();
            if (value == default) continue;
            yield return value;
        }
    }

    public IEnumerable<int> RandomNonDefaultInt(CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
        {
            var value = Random.Shared.Next();
            if (value == default) continue;
            yield return value;
        }
    }

    private IEnumerable<float> RandomNonDefaultFloat(CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
        {
            float value = Random.Shared.NextSingle();
            if (value == default) continue;
            yield return value;
        }
    }

    public object? RandomValuesGenerator(Type propType)
    {
        var func = funcsPerTypes[propType];
        return func;
    }

    public T RandomValueGen<T>()
    {
        var generator = RandomValuesGenerator(typeof(T)) ?? throw new NullReferenceException();
        return UnwrapIEnumerable<T>(generator);
    }

    public object? RandomValue(Type type)
    {
        var mi = this.GetType().GetMethod(nameof(RandomValueGen)) ?? throw new NullReferenceException();
        var gen = mi.MakeGenericMethod(type);
        return gen.Invoke(this, Array.Empty<object>()) ?? throw new NullReferenceException();
    }

    public T UnwrapIEnumerable<T>(object enumerable)
    {
        var func = enumerable as Func<CancellationToken, IEnumerable<T>> ?? throw new NullReferenceException(typeof(T).AssemblyQualifiedName);
        return (func.Invoke(default)).First();
    }
}