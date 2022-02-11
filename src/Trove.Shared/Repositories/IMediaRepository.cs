using Trove.DataModels;
using Trove.Shared.Models;

namespace Trove.Shared.Repositories;

public interface IMediaRepository
{
    Task<Media> GetMediaAsync(Guid id, CancellationToken cancellationToken);
    IAsyncEnumerable<Media> GetRecentMediaAsync(int count, CancellationToken cancellationToken);
    Task<Media> CreateMediaAsync(Media media, CancellationToken cancellationToken);
    Task<Media> UpdateMediaAsync(Media media, CancellationToken cancellationToken);

    Task<FileType> GetFileTypeAsync(Guid id, CancellationToken cancellationToken);
    Task SaveMediaHash(Guid id, byte[] hash, CancellationToken cancellationToken);
}