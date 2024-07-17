using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester.Randoms;

public class GraphGenerator : IObjectGenerator
{
    public static T GenerateBinaryGraphGen<T>(int depthMax, IObjectGenerationOptions opt)
    {
        int depthCurrent = 0;
        return GenerateBinaryGraphWithDepthGen<T>(depthMax, depthCurrent, opt);
    }

    private static object GenerateBinaryGraphWithDepth(Type type, int depthMax, int depthCurrent, IObjectGenerationOptions opt)
    {
        var result = ReflectionsHelperForMethods.MakeGenericCall<GraphGenerator>(nameof(GenerateBinaryGraphWithDepthGen), BindingFlags.Static | BindingFlags.NonPublic, type, null, depthMax, depthCurrent, opt);
        if (result is null) throw new NullReferenceException(type.AssemblyQualifiedName);
        return result;
    }

    private static T GenerateBinaryGraphWithDepthGen<T>(int depthMax, int depthCurrent, IObjectGenerationOptions opt)
    {
        if (opt.FlagsForProperties.HasFlag(BindingFlags.Static)) throw new NotSupportedException("Only instance properties allowed");
        if (opt.FlagsForFields.HasFlag(BindingFlags.Static)) throw new NotSupportedException("Only instance fields allowed");

        var type = typeof(T);
        bool isGeneric = typeof(T).IsGenericType;

        bool isPackToNullable = isGeneric && type.GetGenericTypeDefinition() == typeof(System.Nullable<>);
        if (isPackToNullable)
        {
            type = type.GetGenericArguments().First();
            var nullable = GenerateBinaryGraphWithDepth(type, depthMax, depthCurrent, opt);
            return (T)ReflectionsHelperForNullable.WrapToNullable(type, nullable);
        }

        if (opt.IsUniqGuid && typeof(Guid) == type) return (T)(object)Guid.NewGuid();

        T result = Activator.CreateInstance<T>();

        if (result is null) throw new NullReferenceException(typeof(T).AssemblyQualifiedName);
        if (type.IsClass)
        {
            opt.Context.ScopeOpen(result, type, depthCurrent);
        }


        var props = ReflectionsHelperForProperties.GetAllInstanceProperties(type, opt.FlagsForProperties);

        if (opt.IsIncludeProps)
        {
            foreach (var prop in props)
            {
                var propType = prop.PropertyType;

                if (opt.IsConsiderEfCoreNavigationProperty)
                {
                    if (ReflectionsHelperForEfCoreModel.IsReverseNavigationProperty(type, prop))
                    {
                        if (!ReflectionsHelperForGraph.IsDirectDescendant(prop))
                        {
                            continue;
                        }
                    }
                }

                bool isSelfNested = propType == type;
                if (isSelfNested && !opt.IsAllowSelfNesting) continue;

                if (propType == typeof(string))
                {
                    if (opt.IsUniqString)
                    {
                        prop.SetValue(result, Guid.NewGuid().ToString());
                    }
                    else
                    {
                        prop.SetValue(result, LoremNET.Lorem.Words(2));
                    }

                    continue;
                }

                if (propType.IsClass)
                {
                    if (depthMax == depthCurrent) continue;
                }

                if (ReflectionsHelperForProperties.IsInstanceGotBackingField(prop))
                {
                    if (ReflectionsHelperForTypes.IsEnumerable(propType))
                    {
                        var eble = GenerateEnumerableForGraph(propType, depthMax, depthCurrent, opt.CountEnumerables, opt);
                        prop.SetValue(result, eble);
                    }
                    else
                    {
                        if (type.IsClass)
                        {
                            if (opt.Context.IsMutalReferences(type, propType, prop))
                            {
                                if (opt.Context.IsOpened(propType))
                                {
                                    var mutalValue = opt.Context.GetOpened(propType);
                                    prop.SetValue(result, mutalValue);
                                    continue;
                                }
                            }
                        }

                        var value = GenerateBinaryGraphWithDepth(propType, depthMax, depthCurrent + 1, opt);
                        prop.SetValue(result, value);
                    }
                }
                else continue;
            }
        }

        var fields = ReflectionsHelperForFields.GetAllInstanceFields(type, opt.FlagsForFields);

        foreach (var field in fields)
        {
            var fieldType = field.FieldType;
            bool isSelfNested = fieldType == type;
            if (isSelfNested && !opt.IsAllowSelfNesting) continue;

            if (fieldType.IsClass)
            {
                if (depthMax == depthCurrent) continue;
            }

            if (fieldType == typeof(string))
            {
                if (opt.IsUniqString)
                {
                    field.SetValue(result, Guid.NewGuid().ToString());
                }
                else
                {
                    field.SetValue(result, LoremNET.Lorem.Words(2));
                }

                continue;
            }

            // if (!ReflectionsHelperForGraph.IsDirectDescendant(type, field))
            // {
            //     continue;
            // }

            if (opt.IsIncludeProps && !ReflectionsHelperForFields.IsNotAutoPropertyField(field)) continue;

            if (ReflectionsHelperForTypes.IsEnumerable(fieldType))
            {
                // для коллекций не изменяем глубину, на которой они были созданы
                var eble = GenerateEnumerableForGraph(fieldType, depthMax, depthCurrent, opt.CountEnumerables, opt);
                field.SetValue(result, eble);
            }
            else
            {
                if (type.IsClass)
                {
                    if (opt.Context.IsMutalReferences(type, fieldType, field))
                    {
                        if (opt.Context.IsOpened(fieldType))
                        {
                            var mutalValue = opt.Context.GetOpened(fieldType);
                            field.SetValue(result, mutalValue);
                            continue;
                        }
                    }
                }

                var value = GenerateBinaryGraphWithDepth(fieldType, depthMax, depthCurrent + 1, opt);
                field.SetValue(result, value);
            }

            if (fieldType.IsClass && opt.Context.RenewDepthForReferenceType(fieldType, depthCurrent, field, result))
            {
                // объект был создан ранее   
                // opt.Context.ClearNavigationPropertiesForDependecies(fieldType);
            }
        }

        if (opt.IsConsiderEfCoreNavigationProperty)
        {
            var pair = ReflectionsHelperForEfCoreModel.GetIdAndNavigationPropertyTypesAsFields(type);
            if (ReflectionsHelperForEfCoreModel.IsReverseNavigationProperty(type, pair.Item2))
            {
                throw new NotImplementedException();
            }
        }


        if (type.IsClass)
        {
            opt.Context.ScopeClose(type);
        }


        return result;
    }


    public static object GenerateEnumerableForGraph(Type enumarableType, int depthMax, int depthCurrent, int count, IObjectGenerationOptions opt)
    {
        if (count < 0) throw new ArgumentException($"count must be 0 or greater");
        Type? itemType = default;
        if (enumarableType.IsGenericType)
        {
            var enumarableTypeDef = enumarableType.GetGenericTypeDefinition();
            var args = enumarableType.GetGenericArguments();

            if (args.Length == 1)
            {
                itemType = args.First();

                if (enumarableType.IsInterface)
                {
                    if (enumarableTypeDef == typeof(IList<>))
                    {
                        return GenerateListForGraph(itemType, depthMax, depthCurrent, count, opt);
                    }
                    else if (enumarableTypeDef == typeof(IDictionary<,>))
                    {
                        throw new NotImplementedException();
                    }
                    else if (enumarableTypeDef == typeof(ICollection<>))
                    {
                        return GenerateListForGraph(itemType, depthMax, depthCurrent, count, opt);
                    }
                    else
                    {
                        return GenerateArrayForGraph(itemType, depthMax, depthCurrent, count, opt);
                    }
                }
                else
                {
                    var typeDef = enumarableType.GetGenericTypeDefinition();
                    // кастомные или 
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        else
        {
            if (enumarableType == typeof(Array))
            {
                itemType = enumarableType.GetElementType() ?? throw new NullReferenceException("Impossible feat");
                return GenerateArrayForGraph(itemType, depthMax, depthCurrent, count, opt);
            }

            if (enumarableType.IsInterface)
            {
                if (enumarableType == typeof(IDictionary))
                {
                    throw new NotImplementedException();
                }
                else if (enumarableType == typeof(IList))
                {
                    // можно вернуть array
                    throw new NotImplementedException();
                }
                else if (enumarableType == typeof(ICollection))
                {
                    var result = ReflectionsHelperForMethods.MakeGenericCall<GraphGenerator>(nameof(GenerateArrayForGraphGen), BindingFlags.Static | BindingFlags.Public, itemType, null, depthMax, depthCurrent, count, opt);
                    if (result is null) throw new NullReferenceException();
                    return result;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                var container = Activator.CreateInstance(enumarableType);
                // fill container is there is setters available
                throw new NotImplementedException();
            }
        }
    }

    public static object GenerateListForGraph(Type itemType, int depthMax, int depthCurrent, int count, IObjectGenerationOptions opt)
    {
        return ReflectionsHelperForMethods.MakeGenericCall<GraphGenerator>(nameof(GenerateListForGraphGen), BindingFlags.Static | BindingFlags.Public, itemType, null, depthMax, depthCurrent, count, opt) ?? throw new NullReferenceException();
    }

    public static object GenerateArrayForGraph(Type itemType, int depthMax, int depthCurrent, int count, IObjectGenerationOptions opt)
    {
        return ReflectionsHelperForMethods.MakeGenericCall<GraphGenerator>(nameof(GenerateArrayForGraphGen), BindingFlags.Static | BindingFlags.Public, itemType, null, depthMax, depthCurrent, count, opt) ?? throw new NullReferenceException();
    }

    public static T[] GenerateArrayForGraphGen<T>(int depthMax, int depthCurrent, int count, IObjectGenerationOptions opt)
    {
        var a = new T[count];
        for (int i = 0; i < count; i++)
        {
            var item = GenerateBinaryGraphWithDepthGen<T>(depthMax, depthCurrent + 1, opt);
            a[i] = item;
        }

        return a;
    }

    public static List<T> GenerateListForGraphGen<T>(int depthMax, int depthCurrent, int count, IObjectGenerationOptions opt)
    {
        var list = new List<T>(count);

        for (int i = 0; i < count; i++)
        {
            var item = GenerateBinaryGraphWithDepthGen<T>(depthMax, depthCurrent + 1, opt);
            list.Add(item);
        }

        return list;
    }
}