using MongoDB.Bson.Serialization.Attributes;

namespace Trove.DataAccess.MongoDB.Models;

internal class MongoMedia
{
    [BsonId]
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }

    public MongoSize? Size { get; set; }
    public MongoFileType? FileType { get; set; }


    public Uri? ImageUri { get; set; }

    public byte[]? FileHash { get; set; }

    public MongoTimestamped<bool>? Approved { get; set; }
    public MongoTimestamped<bool>? Deleted { get; set; }
    public MongoTimestamped<bool>? Mature { get; set; }

    public DateTime CreatedAt { get; set; }
}