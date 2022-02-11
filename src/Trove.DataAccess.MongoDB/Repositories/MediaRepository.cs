using System.Runtime.CompilerServices;
using MongoDB.Driver;
using Trove.DataAccess.MongoDB.Mappers;
using Trove.DataAccess.MongoDB.Models;
using Trove.DataModels;
using Trove.Shared.Exception;
using Trove.Shared.Models;
using Trove.Shared.Repositories;

namespace Trove.DataAccess.MongoDB.Repositories;

public class MediaRepository : IMediaRepository
{
    private readonly ContextFactory _contextFactory;
    private readonly IMongoCollection<MongoMedia> _mediaStore;
    private readonly IMongoCollection<MongoSource> _sourceStore;

    public MediaRepository(ContextFactory contextFactory)
    {
        _contextFactory = contextFactory;

        _mediaStore = _contextFactory.GetContext<MongoMedia>(CollectionNames.MediaCollection);
        _sourceStore = _contextFactory.GetContext<MongoSource>(CollectionNames.SourceCollection);
    }

    public async Task<Media> GetMediaAsync(Guid id, CancellationToken cancellationToken)
    {
        MongoMedia? mongoMedia = await _mediaStore
            .Find(m => m.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

        if (mongoMedia == null || (mongoMedia.Deleted?.Value ?? false))
            throw new EntryNotFoundException(id);

        return MediaMapper.Map(mongoMedia);
    }

    public async IAsyncEnumerable<Media> GetRecentMediaAsync(int count, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        using IAsyncCursor<MongoMedia>? cursor = await _mediaStore.Find(m => m.Deleted == null || !m.Deleted.Value)
            .SortByDescending(m => m.CreatedAt)
            .Limit(count)
            .ToCursorAsync(cancellationToken);

        while (await cursor.MoveNextAsync(cancellationToken))
        {
            foreach (MongoMedia mongoMedia in cursor.Current)
            {
                yield return MediaMapper.Map(mongoMedia);
            }
        }
    }

    public async Task<Media> CreateMediaAsync(Media media, CancellationToken cancellationToken)
    {
        MongoMedia? existingMedia = await _mediaStore
            .Find(d => d.ImageUri == media.ImageUri && (d.Deleted == null || !d.Deleted.Value))
            .SingleOrDefaultAsync(cancellationToken);

        if (existingMedia != null)
            throw new EntryAlreadyExistsException(existingMedia.Id, "An entry already exists with the supplied ImageUri");

        MongoMedia mongoMedia = MediaMapper.MapLight(media);
        mongoMedia.CreatedAt = DateTime.UtcNow;

        await _mediaStore.InsertOneAsync(mongoMedia, null, cancellationToken);

        return MediaMapper.Map(mongoMedia);
    }

    public Task<Media> UpdateMediaAsync(Media media, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<FileType> GetFileTypeAsync(Guid id, CancellationToken cancellationToken)
    {
        MongoFileType? fileType = await _mediaStore
            .Find(d => d.Id == id && (d.Deleted == null || !d.Deleted.Value))
            .Project(m => m.FileType)
            .SingleOrDefaultAsync(cancellationToken);

        if (fileType == null)
            throw new EntryNotFoundException(id);

        return FileTypeMapper.Map(fileType);
    }

    public Task SaveMediaHash(Guid id, byte[] hash, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}