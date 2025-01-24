using Deepin.Infrastructure.Extensions;
using Deepin.ServiceDefaults.Extensions;
using Deepin.Storage.API.Application.Services;
using Deepin.Storage.API.Infrastructure;
using Deepin.Storage.API.Infrastructure.Entitites;
using Deepin.Storage.API.Infrastructure.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace Deepin.Storage.API.Extensions;

public static class HostExtensions
{
    public static WebApplicationBuilder AddApplicationService(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("DefaultConnection");
        builder.AddServiceDefaults();
        builder.Services
        .AddStorageDbContext(connectionString)
        .AddMigration<StorageDbContext>()
        .AddDefaultUserContexts()
        .AddFileStorage(builder.Configuration);

        return builder;
    }
    public static WebApplication UseApplicationService(this WebApplication app)
    {
        app.UseServiceDefaults();

        app.MapGet("/api/about", () => new
        {
            Name = "Deepin.Storage.API",
            Version = "1.0.0",
            DeepinEnv = app.Configuration["DEEPIN_ENV"],
            app.Environment.EnvironmentName
        });
        return app;
    }
    private static IServiceCollection AddStorageDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<StorageDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        return services;
    }
    private static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var storageProvider = configuration.GetSection("Storage").GetValue<StorageProvider>("Provider");
        switch (storageProvider)
        {
            case StorageProvider.Local:
                services.Configure<LocalFileStorageOptions>(configuration.GetSection("Storage"));
                services.AddScoped<IFileStorage, LocalFileStorage>();
                break;
            case StorageProvider.AmazonS3:
                services.AddScoped<IFileStorage, S3FileStorage>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(storageProvider), storageProvider, null);
        }
        services.AddScoped<IFileService, FileService>();
        return services;
    }
}
