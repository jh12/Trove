using Trove.DataModels.Metadata;

namespace Trove.DataModels;

public class Media
{
    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }

    public Size? Size { get; set; }

    public FileType Type { get; set; } = null!;

    public Uri? ImageUri { get; set; }

    public Media()
    {
        
    }

    public Media(Guid? id, string? title, string? description, Size? size, Uri? imageUri, FileType type)
    {
        Id = id;
        Title = title;
        Description = description;
        Size = size;
        ImageUri = imageUri;
        Type = type;
    }
}