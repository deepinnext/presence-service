using Deepin.Infrastructure.Caching;
using Deepin.Presence.API.Application.Models;

namespace Deepin.Presence.API.Application.Services;

public class PresenceService(ICacheManager cacheManager) : IPresenceService
{
    private readonly ICacheManager _cacheManager = cacheManager;

    public async Task<UserPresence?> GetUserPresenceAsync(string userId)
    {
        return await _cacheManager.GetAsync<UserPresence>(userId);
    }
    public async Task<IEnumerable<UserPresence>?> GetUsersPresenceAsync(IEnumerable<string> userIds)
    {
        return await _cacheManager.GetAsync<UserPresence>([.. userIds]);
    }

    public async Task SetUserOfflineAsync(string userId)
    {
        var presence = await _cacheManager.GetAsync<UserPresence>(userId);
        presence ??= new UserPresence
        {
            UserId = userId,
        };
        presence.Status = PresenceStatus.Offline;
        presence.LastOnlineAt = DateTime.UtcNow;
        await _cacheManager.SetAsync(userId, presence);
    }

    public async Task SetUserOnlineAsync(string userId)
    {
        var presence = await _cacheManager.GetAsync<UserPresence>(userId);
        if (presence == null)
        {
            presence = new UserPresence
            {
                UserId = userId
            };
        }
        presence.Status = PresenceStatus.Online;
        await _cacheManager.SetAsync(userId, presence);
    }

    public async Task SetUserPresenceAsync(string userId, UserPresence presence)
    {
        await _cacheManager.SetAsync(userId, presence,TimeSpan.FromDays(7));
    }

}
