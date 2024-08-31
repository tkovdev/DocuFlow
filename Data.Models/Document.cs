using Data.Access.Abstractions.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Models;

public class Document : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public string Name { get; set; } = String.Empty;
    
    public DocumentFile File { get; set; } = DocumentFile.Empty();

    public List<Signature> Signatures { get; set; } = new List<Signature>();
}