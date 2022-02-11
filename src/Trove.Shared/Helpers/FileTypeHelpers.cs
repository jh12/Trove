using Trove.Shared.Models;

namespace Trove.Shared.Helpers;

public static class FileTypeHelpers
{
    public static string GetMimeType(this FileType fileType)
    {
        return fileType switch
        {
            FileType.Jpeg => "image/jpeg",
            FileType.Png => "image/png",
            FileType.Gif => "image/gif",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}