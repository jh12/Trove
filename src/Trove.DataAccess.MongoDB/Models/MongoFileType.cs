using Trove.Shared.Models;

namespace Trove.DataAccess.MongoDB.Models;

internal class MongoFileType
{
    public FileType Extension { get; set; }
    public string? Class { get; set; }
}