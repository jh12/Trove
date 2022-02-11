using Autofac;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Trove.DataAccess.MongoDB.Models;
using Trove.Shared.Options;

namespace Trove.DataAccess.MongoDB;

public class ContextFactory : IStartable
{
    private readonly MongoDBOptions _mongoDbOptions;

    public ContextFactory(IOptions<MongoDBOptions> mongoDbOptions)
    {
        _mongoDbOptions = mongoDbOptions.Value;
    }

    public IMongoDatabase GetDatabase()
    {
        return GetClient().GetDatabase(_mongoDbOptions.Database);
    }

    public IMongoCollection<T> GetContext<T>(string collectionName)
    {
        IMongoDatabase database = GetDatabase();

        return database.GetCollection<T>(collectionName);
    }

    private MongoClient GetClient()
    {
        return new MongoClient(new MongoClientSettings
        {
            Server = MongoServerAddress.Parse(_mongoDbOptions.Server),
            Credential = MongoCredential.CreateCredential(_mongoDbOptions.Database, _mongoDbOptions.Username, _mongoDbOptions.Password)
        });
    }

    public void Start()
    {
        SeedDatabase();
    }

    private static readonly object Lock = new();
    private static bool _isInitialized;

    private void SeedDatabase()
    {
        lock (Lock)
        {
            if (_isInitialized)
                return;

            MongoClient client = GetClient();

            IMongoDatabase mongoDatabase = client.GetDatabase(_mongoDbOptions.Database);

            List<string> collectionNames = mongoDatabase.ListCollectionNames().ToList();

            // Media
            {
                if (!collectionNames.Contains(CollectionNames.MediaCollection))
                    mongoDatabase.CreateCollection(CollectionNames.MediaCollection);

                IndexKeysDefinition<MongoMedia> createdAtDefinition = Builders<MongoMedia>.IndexKeys.Descending(s => s.CreatedAt);
                mongoDatabase.GetCollection<MongoMedia>(CollectionNames.MediaCollection).Indexes.CreateOne(new CreateIndexModel<MongoMedia>(createdAtDefinition));
            }

            // Source
            {
                if (!collectionNames.Contains(CollectionNames.SourceCollection))
                    mongoDatabase.CreateCollection(CollectionNames.SourceCollection);

                IndexKeysDefinition<MongoSource> siteUriDefinition = Builders<MongoSource>.IndexKeys.Hashed(s => s.SiteUri);

                mongoDatabase.GetCollection<MongoSource>(CollectionNames.SourceCollection).Indexes.CreateOne(new CreateIndexModel<MongoSource>(siteUriDefinition));
            }

            _isInitialized = true;
        }
    }
}