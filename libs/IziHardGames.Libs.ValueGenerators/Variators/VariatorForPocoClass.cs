using System;
using System.Collections.Generic;
using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester;

public class VariatorForPocoClass : IVariator
{
    /// <summary>
    /// для каждого поля устанавливается вариативное значение, при этом другие поля остаются дефолтными.
    /// P.S. если нужно сочетание полей, то надо создавать отдельный конфигурационный объект и пользоваться другим методом (20240607 пока нет)
    /// </summary>
    /// <typeparam name="TPoco"></typeparam>
    /// <returns></returns>
    public IEnumerable<TPoco> VariantsForPoco<TPoco>(IRegistryForVariators registry)
    {
        foreach (var propInfo in typeof(TPoco).GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            foreach (var variant in VariantsForPoco<TPoco>(registry, propInfo))
            {
                yield return variant;
            }
        }
    }

    public IEnumerable<TPoco> VariantsForPoco<TPoco>(IRegistryForVariators registry, PropertyInfo propInfo)
    {
        var variator = registry.GetVariator(propInfo.PropertyType ?? throw new NullReferenceException());

        foreach (var variant in VariantsForPoco<TPoco>(variator, propInfo))
        {
            yield return variant;
        }
    }

    public IEnumerable<TPoco> VariantsForPoco<TPoco>(object funcToGetVariants, PropertyInfo info)
    {
        var type = funcToGetVariants.GetType();
        if (type.GetGenericTypeDefinition() != typeof(Func<>)) throw new ArgumentException("Func<> is expected");
        if (type.GetGenericArguments()[0].GetGenericTypeDefinition() != typeof(IEnumerable<>)) throw new ArgumentException("IEnumerable<> is expected as type parameter");

        var mi = GetType().GetMethod(nameof(VariantsForPocoWithProp), BindingFlags.Instance | BindingFlags.Public) ?? throw new NullReferenceException();
        var gen = mi.MakeGenericMethod(typeof(TPoco), info.PropertyType);
        return (IEnumerable<TPoco>)gen.Invoke(this, new object[] { funcToGetVariants, info })!;
    }

    public IEnumerable<TPoco> VariantsForPocoWithProp<TPoco, TProp>(object funcWrap, PropertyInfo info)
    {
        var func = (Func<IEnumerable<TProp>>)funcWrap;
        foreach (var propValue in func.Invoke())
        {
            var instance = Activator.CreateInstance<TPoco>();
            info.SetValue(instance, propValue);
            yield return instance;
        }
    }
}