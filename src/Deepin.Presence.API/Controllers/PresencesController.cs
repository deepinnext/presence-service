using Deepin.Domain;
using Deepin.Presence.API.Application.Models;
using Deepin.Presence.API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deepin.Presence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PresencesController(IUserContext userContext, IPresenceService presenceService) : ControllerBase
    {
        private readonly IUserContext _userContext = userContext;
        private readonly IPresenceService _presenceService = presenceService;

        [HttpGet]
        public async Task<ActionResult<UserPresence>> GetAsync()
        {
            var presence = await _presenceService.GetUserPresenceAsync(_userContext.UserId);
            if (presence == null)
            {
                return NotFound();
            }
            return Ok(presence);
        }
        [HttpGet("batch")]
        public async Task<ActionResult<IEnumerable<UserPresence>>> GetAsync(string[] userIds)
        {
            var presences = await _presenceService.GetUsersPresenceAsync(userIds);
            if (presences == null)
            {
                return NotFound();
            }
            return Ok(presences);
        }
        [HttpPost("status")]
        public async Task<ActionResult> UpdateStatusAsync([FromForm] CustomStatusRequest request)
        {
            var presence = await _presenceService.GetUserPresenceAsync(_userContext.UserId);
            if (presence == null)
            {
                return NotFound();
            }
            presence.CustomStatus = request.CustomStatus;
            presence.CustomStatusExpiresAt = request.CustomStatusExpiresAt;
            await _presenceService.SetUserPresenceAsync(_userContext.UserId, presence);
            return Ok();
        }
    }
}
