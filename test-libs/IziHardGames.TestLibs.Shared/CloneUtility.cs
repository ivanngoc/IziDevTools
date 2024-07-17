using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Force.DeepCloner;
using ProtoBuf;

namespace IziHardGames.Libs.Tests;

#pragma warning disable
public class CloneUtility
{
    public static T DeepClone<T>(object obj)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }

    public static T DeepCloneWithProtobuf<T>(T obj)
    {
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, obj);
        return Serializer.Deserialize<T>(stream);
    }

    public static T DeepCloneWithDeepCloner<T>(T obj)
    {
        return obj.DeepClone();
    }
}