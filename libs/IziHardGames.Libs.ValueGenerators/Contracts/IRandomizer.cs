using System;

namespace IziHardGames.Libs.Methods.Tester;

/// <summary>
/// Создает объекты 
/// </summary>
public interface IRandomizer : IObjectGenerator
{
    object? RandomValuesGenerator(Type propType);
    T RandomValueGen<T>();
    object? RandomValue(Type type);
}