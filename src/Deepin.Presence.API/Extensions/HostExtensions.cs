using Deepin.Infrastructure.Caching;
using Deepin.Presence.API.Application.Services;
using Deepin.Presence.API.Hubs;
using Deepin.ServiceDefaults.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Deepin.Presence.API.Extensions;

public static class HostExtensions
{
    public static WebApplicationBuilder AddApplicationService(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        builder.Services
        .AddDefaultCache(new RedisCacheOptions
        {
            ConnectionString = builder.Configuration.GetConnectionString("RedisConnection") ?? throw new ArgumentNullException("RedisConnection")
        })
        .AddDefaultUserContexts()
        .AddCustomSignalR(builder.Configuration);

        builder.Services.AddScoped<IPresenceService, PresenceService>();
        return builder;
    }
    public static WebApplication UseApplicationService(this WebApplication app)
    {
        app.UseServiceDefaults();
        app.MapHub<PresencesHub>("/hub/presences");

        app.MapGet("/api/about", () => new
        {
            Name = "Deepin.Presence.API",
            Version = "1.0.0",
            DeepinEnv = app.Configuration["DEEPIN_ENV"],
            app.Environment.EnvironmentName
        });
        return app;
    }
    private static IServiceCollection AddCustomSignalR(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection = configuration.GetConnectionString("RedisConnection");
        if (!string.IsNullOrEmpty(redisConnection))
        {
            services.AddDataProtection(opts =>
            {
                opts.ApplicationDiscriminator = "Deepin.Presence.API";
            })
             .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redisConnection), "Deepin.Presence.API.DataProtection.Keys");

            services.AddSignalR().AddStackExchangeRedis(redisConnection, options => { });
        }
        else
        {
            services.AddSignalR();
        }
        return services;
    }
}
