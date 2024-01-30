using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TeamTasks.Domain.Entities;

public class BaseMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}