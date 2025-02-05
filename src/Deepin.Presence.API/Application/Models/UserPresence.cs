namespace Deepin.Presence.API.Application.Models;

public class UserPresence
{
    public required string UserId { get; set; }
    public PresenceStatus Status { get; set; }
    public DateTime? LastOnlineAt { get; set; }
    public string? CustomStatus { get; set; }
    public DateTime? CustomStatusExpiresAt { get; set; }
}
