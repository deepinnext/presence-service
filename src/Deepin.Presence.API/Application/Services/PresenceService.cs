using System.Text.Json;
using Deepin.Presence.API.Application.Models;
using Deepin.ServiceDefaults.Extensions;
using Microsoft.Extensions.Caching.Distributed;

namespace Deepin.Presence.API.Application.Services;

public class PresenceService(IDistributedCache cache)
{
    private readonly IDistributedCache _cache = cache;

    public async Task<UserPresence?> GetUserPresenceAsync(string userId)
    {
        var presence = await _cache.GetStringAsync(userId);
        return presence == null ? null : JsonSerializer.Deserialize<UserPresence>(presence);
    }
    public async Task<IDictionary<string, UserPresence>> GetUsersPresenceAsync(IEnumerable<string> userIds)
    {
        _cache.
        var presences = await _cache.GetAsync(userIds);
        return presences.ToDictionary(
            p => p.Key,
            p => JsonSerializer.Deserialize<UserPresence>(p.Value));
    }

    public async Task SetUserPresenceAsync(string userId, UserPresence presence)
    {
        var options = new DistributedCacheEntryOptions();
        if (presence.Status == PresenceStatus.Custom)
        {
            options.SetAbsoluteExpiration(presence.CustomStatusExpiresAt.Value);
        }
        await _cache.SetStringAsync(userId, JsonSerializer.Serialize(presence), options);
    }

}
