using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MovieHub.Application.Common.Interfaces;

namespace MovieHub.Infrastructure.Identity;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?
            .FindFirst("userId")?.Value;

        return int.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}