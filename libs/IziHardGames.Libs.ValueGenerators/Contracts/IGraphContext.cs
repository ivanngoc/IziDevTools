using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using IziHardGames.TestLibs.ValueGenerators.ForEfCore;

namespace IziHardGames.Libs.Methods.Tester;

/// <summary>
/// Нужен для связывания объектов которые взаимно ссылаются друг на друга 
/// </summary>
public interface IGraphContext
{
    IEnumerable<DepthInfo> DepthInfos { get; }
    void CreatePair(object left, object right);
    void CreatePairGen<T1, T2>(T1 left, T2 right);
    void ScopeOpen(object instance, Type type, int depth);
    void ScopeClose(Type type);
    bool IsMutalReferences(Type from, Type to, PropertyInfo fromProp);
    bool IsMutalReferences(Type from, Type to, FieldInfo fromField);
    bool IsOpened(string key);
    bool IsOpened(Type key);
    object GetOpened(Type propType);

    /// <summary>
    /// <see langword="true"/> - если объект уже был создан, но появился ниже по иерархии. Это значит что первое создание это навигационное свойство или обратное навигационное свойство/поле 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="depth"></param>
    /// <param name="context"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    bool RenewDepthForReferenceType(Type type, int depth, MemberInfo context, object target);
    void ClearNavigationPropertiesForDependecies(Type fieldType);
}