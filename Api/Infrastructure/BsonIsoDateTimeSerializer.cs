using System;
using api.Domain;
using MongoDB.Bson.Serialization;

namespace api.Infrastructure;

public sealed class BsonIsoDateTimeSerializer : IBsonSerializer<IsoDateTime>
{
    public Type ValueType => typeof(IsoDateTime);

    /// <summary>Deserializes a IsoDateTime value from BSON.</summary>
    public IsoDateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        // MongoDB C# Driver Version (Src: https://github.com/mongodb/mongo-csharp-driver/blob/master/src/MongoDB.Bson/Serialization/Serializers/DateTimeSerializer.cs)
        // string date = MongoDB.Bson.IO.JsonConvert.ToDateTime(context.Reader.ReadString());

        string iso = context.Reader.ReadString();
        return new IsoDateTime(iso);
    }

    /// <summary>Serializes a IsoDateTime value to BSON.</summary>
    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, IsoDateTime value)
    {
        // MongoDB C# Driver Version (Src: https://github.com/mongodb/mongo-csharp-driver/blob/master/src/MongoDB.Bson/Serialization/Serializers/DateTimeSerializer.cs)
        // WriteString(JsonConvert.ToString(value))

        string iso = value.ToString();
        context.Writer.WriteString(iso);
    }

    /// <summary>Deserializes a IsoDateTime value from BSON.</summary>
    object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return Deserialize(context, args);
    }

    /// <summary>Serializes a IsoDateTime value to BSON.</summary>
    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        if (value is IsoDateTime dateTime)
            Serialize(context, args, dateTime);
        else
            throw new ArgumentException($"Expected value of type {typeof(IsoDateTime)}, but got {value?.GetType()}.");
    }
}
