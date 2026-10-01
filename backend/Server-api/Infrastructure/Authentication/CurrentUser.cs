using System.Security.Claims;

namespace Server_api.Infrastructure.Authentication;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public Guid UserId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? httpContextAccessor.HttpContext?.User.FindFirstValue("sub");

            return Guid.TryParse(value, out var userId)
                ? userId
                : throw new UnauthorizedAccessException("The current user is not authenticated.");
        }
    }
}
