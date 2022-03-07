using Microsoft.Extensions.Options;
using Trove.Shared.Exception;
using Trove.Shared.Models;
using Trove.Shared.Repositories;
using FileOptions = Trove.Shared.Options.FileOptions;

namespace Trove.DataAccess.FileSystem.Repositories;

public class FileRepository : IFileRepository
{
    private readonly IMediaRepository _mediaRepository;
    private readonly FileOptions _options;
    private const int DefaultBufferSize = 81920;

    public FileRepository(IMediaRepository mediaRepository, IOptions<FileOptions> options)
    {
        _mediaRepository = mediaRepository;
        _options = options.Value;
    }

    public async Task<FileResult> GetFileAsync(Guid id, CancellationToken cancellationToken)
    {
        string folderPath = DirectoryPathFromId(id);
        string filePath = Path.Combine(folderPath, id.ToString());

        if (!Directory.Exists(folderPath) || !File.Exists(filePath))
            throw new FileDoesNotExistException(id);

        FileType fileType = await _mediaRepository.GetFileTypeAsync(id, cancellationToken);

        FileStream fileStream = File.OpenRead(filePath);

        return new FileResult(fileType, fileStream);
    }

    public async Task SaveFileAsync(Guid id, Stream stream, CancellationToken cancellationToken)
    {
        string folderPath = DirectoryPathFromId(id);
        string filePath = Path.Combine(folderPath, id.ToString());

        Directory.CreateDirectory(folderPath);

        if (File.Exists(filePath))
            throw new FileAlreadyExistsException(id);

        await using (FileStream fileStream = File.OpenWrite(filePath))
        {
            await stream.CopyToAsync(fileStream, DefaultBufferSize, cancellationToken);
        }
    }

    private string DirectoryPathFromId(Guid id)
    {
        string basePath = _options.Storage.Path;

        string idStr = id.ToString();

        return Path.Combine(basePath, idStr[..1], idStr[1..3]);
    }
}