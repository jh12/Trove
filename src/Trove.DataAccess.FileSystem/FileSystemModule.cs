using Autofac;
using Trove.DataAccess.FileSystem.Repositories;
using Trove.Shared.Repositories;

namespace Trove.DataAccess.FileSystem;

public class FileSystemModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<FileRepository>().As<IFileRepository>();
    }
}