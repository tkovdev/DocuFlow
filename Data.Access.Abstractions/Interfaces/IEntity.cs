using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Access.Abstractions.Interfaces;

/**
 * <summary>
 * IEntity represents a MongoDB collection Schema.
 * Any object represented in MongoDB that will be queried should inherit from this object.
 * </summary>
 */
public interface IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
}