using MovieHub.Domain.Entities;

namespace MovieHub.Application.Common.Interfaces.Identity;

public interface IJwtProvider
{
    string GenerateToken(User user);
}