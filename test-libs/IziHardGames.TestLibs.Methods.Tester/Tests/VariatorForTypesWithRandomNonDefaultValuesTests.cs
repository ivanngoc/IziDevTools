using System.Reflection;
using IziHardGames.Libs.Methods.Tester.Randoms;
using Xunit;

namespace IziHardGames.Libs.Methods.Tester;

public class VariatorForTypesWithRandomNonDefaultValuesTests
{
    [Fact]
    private void Test_Randomization_Non_Default()
    {
        var colection = new CollectionsOfNonDefaultRandomizers();
        var variator = new VariatorForTypesWithRandomNonDefaultValues(colection);
        var vars = variator.Variants<SomeClass>(10);

        foreach (var variant in vars)
        {
            Assert.NotNull(variant);
            CheckInstanceNotDefault(variant);
        }
    }

    private void CheckInstanceNotDefault(object instance)
    {
        foreach (var prop in instance.GetType().GetProperties())
        {
            CheckPropertyNotDefault(instance, prop);
        }
    }

    private void CheckPropertyNotDefault(object target, PropertyInfo propertyInfo)
    {
        if (propertyInfo.PropertyType.IsClass || ReflectionPropertyHelper.IsNullable(propertyInfo))
        {
            var value = propertyInfo.GetValue(target);
            Assert.NotNull(value);
            CheckInstanceNotDefault(value);
        }
        else if (propertyInfo.PropertyType.IsPrimitive)
        {
            var exp = Activator.CreateInstance(propertyInfo.PropertyType);
            Assert.NotEqual(exp, propertyInfo.GetValue(target));
        }
        else
        {
            var value = propertyInfo.GetValue(target);
            Assert.NotNull(value);
            CheckInstanceNotDefault(value);
        }
    }
}