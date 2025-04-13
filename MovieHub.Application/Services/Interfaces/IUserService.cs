using MovieHub.Application.Dtos.Authentication;

namespace MovieHub.Application.Services.Interfaces;

public interface IUserService
{
    Task RegisterUserAsync(RegisterUserRequest request);
    Task<string> LoginAsync(LoginUserRequest request);
}