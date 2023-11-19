using System;
using api.Domain;
using MongoDB.Bson.Serialization;

namespace api.Infrastructure;

public sealed class EntityIdSerializer : IBsonSerializer
{
    static readonly Type _valueType = typeof(EntityId);
    public Type ValueType { get => _valueType; }

    // Deserialize from monogodb (to C#)
    public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        string _id = context.Reader.ReadString();
        return new EntityId(_id);
    }

    // Serialize to monogodb
    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        string entityId = (EntityId)value;
        context.Writer.WriteString(entityId);
    }
}
