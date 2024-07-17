using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester;

public class VariatorForModel : IVariator
{
    private VariatorForPocoClass variatorForPocoClass = new VariatorForPocoClass();
    private RegistryForVariators registryForVariators = new RegistryForVariators();

    public VariatorForModel()
    {
    }

    /// <summary>
    ///  
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEnumerable<TModel> GetTestingVariants<TModel>()
    {
        var type = typeof(TModel);

        if (type == typeof(string)) return VariantsForStrings<TModel>();

        if (type.IsClass) return GetVariantsForClassGen<TModel>();

        if (type == typeof(int)) return VariantsForInt<TModel>();
        if (type == typeof(long)) return VariantsForLong<TModel>();
        if (type == typeof(float)) return VariantsForFloat<TModel>();
        if (type == typeof(double)) return VariantsForDouble<TModel>();
        throw new NotImplementedException();
    }

    public IEnumerable<object> GetVariantsForClass(Type type)
    {
        var mi = GetType().GetMethod(nameof(GetVariantsForClassGen), BindingFlags.Instance | BindingFlags.Public) ?? throw new NullReferenceException();
        var gen = mi.MakeGenericMethod(type);
        return (IEnumerable<object>)(gen.Invoke(this, Array.Empty<object>()) ?? throw new NullReferenceException());
    }

    /// <summary>
    /// По одному объекту на каждое зачение одного свойства
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEnumerable<TModel> GetVariantsForClassGen<TModel>()
    {
        var props = typeof(TModel).GetProperties(BindingFlags.Instance | BindingFlags.Public);

        foreach (var prop in props)
        {
            if (!prop.PropertyType.IsValueType || Nullable.GetUnderlyingType(prop.PropertyType) != null)
            {
                var model = Activator.CreateInstance<TModel>();
                prop.SetValue(model, null!);
                yield return model;
            }

            if (prop.PropertyType == typeof(string))
            {
                var strings = registryForVariators.GetVariatorGeneric<string>();

                foreach (var str in strings.Invoke())
                {
                    var instance = Activator.CreateInstance<TModel>();
                    prop.SetValue(instance, str);
                    yield return instance;
                }
            }
            else if (prop.PropertyType.IsPrimitive || registryForVariators.Registered(prop.PropertyType))
            {
                foreach (var model in VariantsForPropertyWithRegistry<TModel>(prop, registryForVariators))
                {
                    yield return model;
                }
            }
            else if (prop.PropertyType.IsClass)
            {
                var values = GetVariantsForClass(prop.PropertyType);
                foreach (var value in values)
                {
                    var instance = Activator.CreateInstance<TModel>();
                    prop.SetValue(instance, value);
                    yield return instance;
                }
            }
            else
            {
                var variantsForStruct = VariantsForStruct(prop.PropertyType);
                foreach (var structModel in (IEnumerable)variantsForStruct)
                {
                    var instance = Activator.CreateInstance<TModel>();
                    prop.SetValue(instance, structModel);
                    yield return instance;
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    /// <exception cref="NullReferenceException"></exception>
    public object VariantsForStruct(Type type)
    {
        var mi = GetType().GetMethod(nameof(VariantsForStructGen), BindingFlags.Instance | BindingFlags.Public) ?? throw new NullReferenceException();
        var gen = mi.MakeGenericMethod(type);
        return (gen.Invoke(this, Array.Empty<object>()) ?? throw new NullReferenceException());
    }

    public IEnumerable<TStruct> VariantsForStructGen<TStruct>()
    {
        return GetVariantsForClassGen<TStruct>();
    }

    public IEnumerable<TModel> VariantsForPropertyWithRegistry<TModel>(PropertyInfo prop, IRegistryForVariators registry)
    {
        var func = registry.GetVariator(prop.PropertyType);
        var mi = GetType().GetMethod(nameof(VariantsForPropertyWithWrap), BindingFlags.Instance | BindingFlags.Public)!;
        var gen = mi.MakeGenericMethod(typeof(TModel), prop.PropertyType);
        return (IEnumerable<TModel>)gen.Invoke(this, new object[] { prop, func })!;
    }

    public IEnumerable<TModel> VariantsForPropertyWithWrap<TModel, TProp>(PropertyInfo prop, object wrap)
    {
        var variator = (Func<IEnumerable<TProp>>)wrap;
        foreach (var variant in variator.Invoke())
        {
            var instance = Activator.CreateInstance<TModel>();
            prop.SetValue(instance, variant);
            yield return instance;
        }
    }

    public IEnumerable<TModel> VariantsForProperty<TModel>(PropertyInfo info)
    {
        return variatorForPocoClass.VariantsForPoco<TModel>(registryForVariators, info);
    }

    private static IEnumerable<TModel> VariantsForStrings<TModel>()
    {
        return VariatorForPrimitive.VariantsForStrings().Cast<TModel>();
    }

    private static IEnumerable<TModel> VariantsForDouble<TModel>()
    {
        return VariatorForPrimitive.VariantsForDouble().Cast<TModel>();
    }

    private static IEnumerable<TModel> VariantsForFloat<TModel>()
    {
        return VariatorForPrimitive.VariantsForFloat().Cast<TModel>();
    }

    private static IEnumerable<TModel> VariantsForLong<TModel>()
    {
        return VariatorForPrimitive.VariantsForLong().Cast<TModel>();
    }

    public static IEnumerable<T> VariantsForInt<T>() => VariatorForPrimitive.VariantsForInt().Cast<T>();

    public static IEnumerable<T> GetSequenceByDefault<T>(int depthMax)
    {
        int depthCurrent = 0;
        // Ra
        throw new NotImplementedException();
    }
}