namespace Trove.Shared.Models;

public sealed class FileResult : IDisposable
{
    public FileType FileType { get; }
    public Stream Stream { get; }

    public string ContentType => FileType switch
    {
        FileType.Jpeg => "image/jpeg",
        FileType.Png => "image/png",
        FileType.Gif => "image/gif",
        _ => throw new ArgumentOutOfRangeException()
    };

    public FileResult(FileType fileType, Stream stream)
    {
        FileType = fileType;
        Stream = stream;
    }

    public void Dispose()
    {
        Stream.Dispose();
    }
}   