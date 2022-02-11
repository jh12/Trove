namespace Trove.DataAccess.MongoDB.Models;

public class MongoTimestamped<T> where T : notnull
{
    public T Value { get; set; } = default!;
    public DateTime ChangedAt { get; set; }
}