using Trove.Shared.Models;

namespace Trove.Shared.Repositories;

public interface IFileRepository
{
    Task<FileResult> GetFileAsync(Guid id, CancellationToken cancellationToken);

    Task SaveFileAsync(Guid id, Stream stream, CancellationToken cancellationToken);
}