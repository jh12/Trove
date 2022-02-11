using Trove.DataAccess.MongoDB.Models;
using Trove.Shared.Models;

namespace Trove.DataAccess.MongoDB.Mappers;

internal static class FileTypeMapper
{
    public static FileType Map(MongoFileType fileType)
    {
        return fileType.Extension;
    }
}