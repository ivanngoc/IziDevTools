using System;
using System.Collections.Generic;
using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester;

public class RegistryForVariators : IRegistryForVariators
{
    /// <summary>
    /// Value = <see cref="Func{TResult}"/>
    /// </summary>
    private readonly Dictionary<Type, object> funcsPerTypes = new Dictionary<Type, object>();

    public RegistryForVariators()
    {
        funcsPerTypes.Add(typeof(string), VariatorForPrimitive.CreateFuncAs(nameof(VariatorForPrimitive.VariantsForStrings), typeof(string)));
        funcsPerTypes.Add(typeof(int), VariatorForPrimitive.CreateFuncAs(nameof(VariatorForPrimitive.VariantsForInt), typeof(int)));
        funcsPerTypes.Add(typeof(long), VariatorForPrimitive.CreateFuncAs(nameof(VariatorForPrimitive.VariantsForLong), typeof(long)));
        funcsPerTypes.Add(typeof(float), VariatorForPrimitive.CreateFuncAs(nameof(VariatorForPrimitive.VariantsForFloat), typeof(float)));
        funcsPerTypes.Add(typeof(double), VariatorForPrimitive.CreateFuncAs(nameof(VariatorForPrimitive.VariantsForDouble), typeof(double)));
        funcsPerTypes.Add(typeof(DateTime), VariatorForBcl.CreateFuncAs(nameof(VariatorForBcl.VariantsForDateTime), typeof(DateTime)));
    }

    public Func<IEnumerable<T>> GetVariatorGeneric<T>()
    {
        if (funcsPerTypes.TryGetValue(typeof(T), out var handler))
        {
            return (Func<IEnumerable<T>>)handler;
        }

        throw new ArgumentOutOfRangeException($"Variator for type {typeof(T).AssemblyQualifiedName} Not Founded");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns>Func&lt;IEnumerable&lt;T&gt;&gt;<br/><see cref="Func{TResult}"/> as object Where TResult is <see cref="IEnumerable{T}"/> where T is <see cref="type"/></returns>
    /// <exception cref="NullReferenceException"></exception>
    public object GetVariator(Type type)
    {
        var mi = typeof(RegistryForVariators).GetMethod(nameof(GetVariatorGeneric), BindingFlags.Instance | BindingFlags.Public) ?? throw new NullReferenceException();
        var gen = mi.MakeGenericMethod(type);
        return gen.Invoke(this, Array.Empty<object>()) ?? throw new NullReferenceException("Must return Func<IEnumerable<Type>>");
    }

    public Delegate GetVariatorAsDelegate(Type type)
    {
        throw new NotImplementedException();
    }

    public bool Registered(Type type)
    {
        return funcsPerTypes.ContainsKey(type);
    }

    public bool TryGetAs<T>(Type type, out T variator)
    {
        var result = funcsPerTypes.TryGetValue(type, out var obj);
        variator = (T)obj!;
        return result;
    }
}