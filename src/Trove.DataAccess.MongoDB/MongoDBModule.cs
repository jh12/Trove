using Autofac;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Trove.DataAccess.MongoDB.Repositories;
using Trove.Shared.Repositories;

namespace Trove.DataAccess.MongoDB;

public class MongoDBModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ContextFactory>().As<IStartable>().AsSelf();

        builder.RegisterType<MediaRepository>().As<IMediaRepository>().SingleInstance();

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        // LinQ queries seems broken if this obsolete property is not set
#pragma warning disable 618
        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
#pragma warning restore 618
    }
}