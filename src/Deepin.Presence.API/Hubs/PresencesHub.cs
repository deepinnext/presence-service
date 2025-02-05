using System.Security.Claims;
using Deepin.Presence.API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Deepin.Presence.API.Hubs;

[Authorize]
public class PresencesHub(IPresenceService presenceService) : Hub
{
    private readonly IPresenceService _presenceService = presenceService;
    private string? _userId = null;
    public string? UserId
    {
        get
        {
            if (string.IsNullOrEmpty(_userId))
            {
                _userId = Context.User?.FindFirst("sub")?.Value;
            }
            if (string.IsNullOrEmpty(_userId))
            {
                _userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            return _userId;
        }
    }
    public override async Task OnConnectedAsync()
    {
        if (UserId == null)
        {
            throw new InvalidOperationException("User ID not found.");
        }
        await _presenceService.SetUserOnlineAsync(UserId);
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (UserId == null)
        {
            throw new InvalidOperationException("User ID not found.");
        }
        await _presenceService.SetUserOfflineAsync(UserId);
        await base.OnDisconnectedAsync(exception);
    }
}
