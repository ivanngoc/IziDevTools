using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IziHardGames.Libs.Methods.Tester;

public partial class ReflectionsHelperForGraph
{
    /// <summary>
    /// <see langword="true"/> - свойство является навигационным и рсстояние в иерархии между ними минимальное.
    /// то есть тип является родителем, а тип свойства - дочерним объектом.
    /// проверка нужна для построения правильной иерархии - сверху вниз, а затем заполнение навигационных полей <br/>
    /// объект который не встречается в виде поля ни в каком типе ниже по иерархии типов считается прямым потомком объекта?
    /// работает только если нет никаких обратных свойств
    /// </summary>
    /// <param name="type"></param>
    /// <param name="prop"></param>
    /// <returns></returns>
    public unsafe static bool IsDirectDescendant(PropertyInfo target)
    {
        Type type = target.DeclaringType;
        var targetType = target.PropertyType;
        if (!targetType.IsClass) return true;
        if (targetType == typeof(string)) return true;
        // self nested
        if (type == target.PropertyType) return false;
        List<Type> head = new();
        var props = ReflectionsHelperForProperties.GetAllInstanceProperties(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var prop in props)
        {
            if (prop.PropertyType == type) continue;
            if (prop == target) continue;
            var finded = FindAsPropertyAtHierarchy(prop.PropertyType, target.PropertyType, 1000, 0, head);
            if (finded != null)
            {
                return false;
            }
        }

        return true;
    }

    private unsafe static PropertyInfo? FindAsPropertyAtHierarchy(Type currentNode, Type toFind, int depthMax, int depthCurrent, List<Type> excluded)
    {
        // Console.WriteLine(currentNode.AssemblyQualifiedName);
        if (depthCurrent > depthMax) throw new ArgumentOutOfRangeException($"Max depth reached: {depthMax}");
        excluded.Add(currentNode);
        var props = ReflectionsHelperForProperties.GetAllInstanceProperties(currentNode, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var prop in props)
        {
            if (excluded.Contains(prop.PropertyType)) continue;
            if (prop.DeclaringType == toFind) return prop;
            var finded = FindAsPropertyAtHierarchy(prop.PropertyType!, toFind, depthMax, depthCurrent + 1, excluded);
            if (finded != null) return finded;
        }

        return null;
    }

    public static bool IsDirectDescendant(Type type, FieldInfo field)
    {
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        if (fields.Any(x => x.FieldType == type))
        {
            return true;
        }

        return false;
    }
}