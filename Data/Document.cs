using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data;

public class Document
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    
    public string? Location { get; set; }

    public List<Signature> Signatures { get; set; } = new List<Signature>();
}