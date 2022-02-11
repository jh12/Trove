using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using Trove.Helpers;
using Octothorp.AspNetCore.Helpers;
using Trove.Middleware;
using ServiceCollectionHelper = Trove.Helpers.ServiceCollectionHelper;

var builder = WebApplication.CreateBuilder(args);

// Host
var hostBuilder = builder.Host;
hostBuilder.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(ctx.Configuration);
});
hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
hostBuilder.ConfigureContainer<ContainerBuilder>(ServiceCollectionHelper.RegisterModules);

// ServiceCollection
IServiceCollection services = builder.Services;

services.AddMemoryCache();
services.AddConfigurationSections(builder.Configuration);
services.AddControllers(options =>
    {
        options.Filters.Add<HttpResponseExceptionFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

services.AddCommonSwagger();

// Webapplication
WebApplication application = builder.Build();

application.UseHttpsRedirection();

application.UseSerilogRequestLogging();

application.UseCommonSwagger(options =>
{

}, uiOptions =>
{
    uiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    uiOptions.RoutePrefix = string.Empty;
});

application.MapControllers();
application.MapStatus();

application.Run();