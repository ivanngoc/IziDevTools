using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IziHardGames.Libs.ForJson;

public class PropSelectJsonConverter<TSource, TTarget> : JsonConverter<TSource>
{
    private string propertyName;

    public PropSelectJsonConverter(string propertyName)
    {
        this.propertyName = propertyName;
    }

    public override TSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Implement read logic if needed
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, TSource value, JsonSerializerOptions options)
    {
        var props = value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var prop =  props.First(x=>x.Name == propertyName);
        JsonSerializer.Serialize(writer, prop.GetValue(value), options);
    }
}