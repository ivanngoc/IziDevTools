using System;
using System.Text.Json.Serialization;

namespace IziHardGames.Libs.ForJson;

/// <summary>
/// 
/// </summary>
/// <example>
/// </example>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SelectPropertyJsonConverterAttribute : JsonConverterAttribute
{
    private readonly Type converterType;
    private readonly string propertyName;

    public SelectPropertyJsonConverterAttribute(Type type, string propertyName)
    {
        this.converterType = type;
        this.propertyName = propertyName;
    }
    // public FuncJsonConverterAttribute(Type converterType, params object[] converterArgs)
    // {
    //     _converterType = converterType;
    //     _converterArgs = converterArgs;
    // }

    public override JsonConverter CreateConverter(Type typeToConvert)
    {
        return (JsonConverter)(Activator.CreateInstance(converterType, propertyName) ?? throw new NullReferenceException());
    }
}