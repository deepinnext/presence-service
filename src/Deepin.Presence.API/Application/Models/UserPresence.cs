namespace Deepin.Presence.API.Application.Models;

public class UserPresence
{
    public PresenceStatus Status { get; set; }
    public DateTime? LastOnline { get; set; }
    public DateTime? CustomStatusExpiresAt { get; set; }
}
