using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.Methods.Tester;

public class VariatorForTypesWithRandomNonDefaultValues : IVariator
{
    private readonly ICollectionOfRandomizers collectionOfRandomizers;

    public VariatorForTypesWithRandomNonDefaultValues(ICollectionOfRandomizers collectionOfRandomizers)
    {
        this.collectionOfRandomizers = collectionOfRandomizers;
    }

    public IEnumerable<TModel> Variants<TModel>(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return Variant<TModel>();
        }
    }

    public object VariantForType(Type type)
    {
        var value = Activator.CreateInstance(type) ?? throw new NullReferenceException();
        return Randomize(value);
    }

    public TModel Randomize<TModel>(TModel instance)
    {
        var type = typeof(TModel);
        var props = type.GetProperties();

        foreach (var prop in props)
        {
            var propType = prop.PropertyType;

            if (propType.IsPrimitive)
            {
                var values = collectionOfRandomizers.GetRandomizer(propType).RandomValue(propType) ?? throw new NullReferenceException();
                prop.SetValue(instance, values);
            }
            else if (propType == typeof(string))
            {
                var value = collectionOfRandomizers.GetRandomizer(propType).RandomValue(propType) ?? throw new NullReferenceException();
                prop.SetValue(instance, value);
            }
            else if (propType.IsClass || propType.IsValueType)
            {
                var value = VariantForType(propType);
                prop.SetValue(instance, value);
            }
            else throw new NotImplementedException($"No variator for property type of:{propType.AssemblyQualifiedName}");
        }

        return instance;
    }

    public TModel Variant<TModel>()
    {
        var instance = Activator.CreateInstance<TModel>();
        return Randomize(instance);
    }
}