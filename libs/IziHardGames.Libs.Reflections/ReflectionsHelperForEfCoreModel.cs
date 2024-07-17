using System.Reflection;

namespace IziHardGames.Libs.Methods.Tester;

public class ReflectionsHelperForEfCoreModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>
    /// Item1 is type of Id Field if presented <br/>
    /// Item2 is type of NavigationProperty 
    /// </returns>
    /// <exception cref="NotImplementedException"></exception>
    public static (PropertyInfo?, PropertyInfo) GetIdAndNavigationPropertyTypes(Type model)
    {
        throw new NotImplementedException();
    }

    public static (FieldInfo?, FieldInfo) GetIdAndNavigationPropertyTypesAsFields(Type model)
    {
        throw new NotImplementedException();
    }
    public static bool IsReverseNavigationProperty(Type type, FieldInfo fieldInfo)
    {
        throw new NotImplementedException();
    }
    public static bool IsReverseNavigationProperty(Type type, PropertyInfo propertyInfo)
    {
        throw new NotImplementedException();
    }

    public static EType GetRelationType(Type modelLeft, Type modelRight)
    {
        throw new NotImplementedException();
    }

    

    public enum EType
    {
        None,
        OneToOne,
        OneToMany,
        ManyToMany,
    }
}