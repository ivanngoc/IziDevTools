using System;

namespace IziHardGames.Libs.Methods.Tester.Randoms;

public class ObjectWithRandomValues : IRandomizer
{
    public object? RandomValuesGenerator(Type propType)
    {
        throw new NotImplementedException();
    }

    public T RandomValueGen<T>()
    {
        throw new NotImplementedException();
    }

    public object? RandomValue(Type type)
    {
        throw new NotImplementedException();
    }
}