using Trove.DataAccess.MongoDB.Models;
using Trove.DataModels;
using Trove.DataModels.Metadata;
using SFileType = Trove.Shared.Models.FileType;

namespace Trove.DataAccess.MongoDB.Mappers;

internal static class MediaMapper
{
    public static Media Map(MongoMedia media)
    {
        Size? size = media.Size != null ? new Size(media.Size.Height, media.Size.Width) : null;
        FileClass fileClass = Enum.Parse<FileClass>(media.FileType?.Class ?? string.Empty);

        FileType fileType = new FileType(fileClass, media.FileType?.Extension.ToString() ?? string.Empty);

        return new Media(
            media.Id,
            media.Title,
            media.Description,
            size,
            media.ImageUri,
            fileType);
    }

    public static MongoMedia MapLight(Media media)
    {
        MongoSize? size = media.Size != null ? new MongoSize { Height = media.Size.Height, Width = media.Size.Width } : null;
        MongoFileType fileType = new() { Class = media.Type.FileClass.ToString(), Extension = Enum.Parse<SFileType>(media.Type.Extension) };

        return new MongoMedia
        {
            Title = media.Title,
            Description = media.Description,
            Approved = new MongoTimestamped<bool>(),
            Size = size,
            ImageUri = media.ImageUri,
            FileType = fileType
        };
    }
}