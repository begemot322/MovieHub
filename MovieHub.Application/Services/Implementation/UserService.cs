using System.Security.Authentication;
using MovieHub.Application.Common.Exceptions;
using MovieHub.Application.Common.Interfaces;
using MovieHub.Application.Common.Interfaces.Identity;
using MovieHub.Application.Dtos.Authentication;
using MovieHub.Application.Services.Interfaces;
using MovieHub.Domain.Entities;

namespace MovieHub.Application.Services.Implementation;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    
    public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task RegisterUserAsync(RegisterUserRequest request)
    {
        if (await _unitOfWork.Users.ExistsAsync(u => u.Email == request.Email))
            throw new DuplicateException("Пользователь с таким email уже существует");
        
        var hashedPassword = _passwordHasher.Generate(request.Password);
        
        var user = new User
        {
            Username = request.UserName,
            Email = request.Email,
            PasswordHash = hashedPassword,
        };
        
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<string> LoginAsync(LoginUserRequest request)
    {
        var user = await _unitOfWork.Users.GetByExpressionAsync(u => u.Email == request.Email);
        
        if (user == null)
            throw new NotFoundException($"Пользователь с почтой {request.Email} не найден");
        
        var result = _passwordHasher.Verify(request.Password, user.PasswordHash);
        
        if (result == false)
        {
            throw new AuthenticationException("Неверный логин или пароль");
        }
        
        var token = _jwtProvider.GenerateToken(user);

        return token;
    }
}