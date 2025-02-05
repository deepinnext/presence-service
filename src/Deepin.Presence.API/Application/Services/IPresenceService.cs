using System;
using Deepin.Presence.API.Application.Models;

namespace Deepin.Presence.API.Application.Services;

public interface IPresenceService
{
    Task<UserPresence?> GetUserPresenceAsync(string userId);
    Task<IEnumerable<UserPresence>?> GetUsersPresenceAsync(IEnumerable<string> userIds);
    Task SetUserPresenceAsync(string userId, UserPresence presence);
    Task SetUserOnlineAsync(string userId);
    Task SetUserOfflineAsync(string userId);
}
