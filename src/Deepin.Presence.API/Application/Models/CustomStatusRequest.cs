using System;

namespace Deepin.Presence.API.Application.Models;

public class CustomStatusRequest
{
    public required string CustomStatus { get; set; }
    public DateTime CustomStatusExpiresAt { get; set; }
}
