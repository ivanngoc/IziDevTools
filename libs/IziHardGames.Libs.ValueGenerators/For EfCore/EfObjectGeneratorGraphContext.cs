using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IziHardGames.Libs.Methods.Tester;

namespace IziHardGames.TestLibs.ValueGenerators.ForEfCore;

public class EfObjectGeneratorGraphContext : IGraphContext, IDisposable
{
    private readonly Dictionary<string, Scope> opensByKey = new();
    private readonly Dictionary<Type, DepthInfo> depthInfos = new();
    public IEnumerable<DepthInfo> DepthInfos => depthInfos.Values;

    public void CreatePair(object left, object right)
    {
        throw new NotImplementedException();
    }

    public void CreatePairGen<T1, T2>(T1 left, T2 right)
    {
        throw new NotImplementedException();
    }

    public void ScopeOpen(object instance, Type type, int depth)
    {
        opensByKey.Add(type.AssemblyQualifiedName!, new() { openedAtDepth = depth, type = type, instance = instance });
    }

    public void ScopeClose(Type type)
    {
        opensByKey.Remove(type.AssemblyQualifiedName!);
    }

    public bool IsMutalReferences(Type from, Type to, PropertyInfo fromProp)
    {
        var props = to.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (props.Any(x => x.PropertyType == from))
        {
            return true;
        }

        return false;
    }

    public bool IsMutalReferences(Type from, Type to, FieldInfo fromField)
    {
        var fields = to.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (fields.Any(x => x.FieldType == from))
        {
            return true;
        }

        return false;
    }

    public bool IsOpened(Type type) => opensByKey.ContainsKey(type.AssemblyQualifiedName!);

    public bool IsOpened(string key) => opensByKey.ContainsKey(key);

    public object GetOpened(Type propType)
    {
        return opensByKey[propType.AssemblyQualifiedName!].instance;
    }

    public bool RenewDepthForReferenceType(Type type, int depth, MemberInfo context, object target)
    {
        if (!type.IsClass) throw new ArgumentException(type.AssemblyQualifiedName);
        if (depthInfos.TryGetValue(type, out var info))
        {
            //
        }
        else
        {
            info = new DepthInfo()
            {
                type = type,
                maxDepth = depth
            };
            depthInfos.Add(type, info);
        }

        info.AddMemberInfo(depth, context, target);

        if (info.maxDepth < depth)
        {
            info.maxDepth = depth;
            return true;
        }

        return false;
    }

    public void ClearNavigationPropertiesForDependecies(Type fieldType)
    {
        depthInfos[fieldType].ClearDependecies();
    }

    public void Dispose()
    {
        opensByKey.Clear();
    }
}

public class Scope
{
    public int openedAtDepth;
    public Type type;
    public object instance;
}

public class DepthInfo
{
    public int maxDepth;
    public Type type;
    private List<References> references = new();

    public void AddMemberInfo(int depth, MemberInfo member, object target)
    {
        references.Add(new References()
        {
            depth = depth,
            info = member,
            target = target,
        });
    }

    public void ClearDependecies()
    {
        foreach (var reference in references)
        {
            if (reference.info is PropertyInfo prop)
            {
                prop.SetValue(reference.target, default);
            }
            else if (reference.info is FieldInfo field)
            {
                field.SetValue(reference.target, default);
            }
        }
    }
}

public class References
{
    public int depth;
    public object target;
    public MemberInfo info;
}