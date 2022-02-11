using MongoDB.Bson.Serialization.Attributes;

namespace Trove.DataAccess.MongoDB.Models;

[BsonIgnoreExtraElements]
internal class MongoSource
{
    [BsonId]
    public Guid Id { get; set; }
    public MongoAuthor? Author { get; set; }

    public string? Title { get; set; }

    public Uri SiteUri { get; set; } = null!;
    public Uri? RefererUri { get; set; }

    public bool Deleted { get; set; }
}