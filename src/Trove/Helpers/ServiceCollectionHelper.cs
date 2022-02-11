using Autofac;
using Trove.DataAccess.FileSystem;
using Trove.DataAccess.MongoDB;
using Trove.Shared.Options;
using FileOptions = Trove.Shared.Options.FileOptions;

namespace Trove.Helpers;

public static class ServiceCollectionHelper
{
    public static IServiceCollection AddCaching(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMemoryCache();

        return serviceCollection;
    }

    public static IServiceCollection AddConfigurationSections(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddOptions();

        serviceCollection.Configure<FileOptions>(configuration.GetSection(FileOptions.Key));
        serviceCollection.Configure<MongoDBOptions>(configuration.GetSection(MongoDBOptions.Key));

        return serviceCollection;
    }

    public static T ConfigureAndGet<T>(this IConfiguration configuration, IServiceCollection services, string sectionName) where T : class
    {
        if (string.IsNullOrWhiteSpace(sectionName))
            throw new ArgumentException("Section name cannot be empty", nameof(sectionName));

        IConfigurationSection section = configuration.GetSection(sectionName);
        services.Configure<T>(section);

        return section.Get<T>();
    }

    public static void RegisterModules(HostBuilderContext ctx, ContainerBuilder builder)
    {
        builder
            .RegisterModule<MongoDBModule>()
            .RegisterModule<FileSystemModule>();
    }
}