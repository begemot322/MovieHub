using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Application.Dtos.Authentication;
using MovieHub.Application.Services.Interfaces;

namespace MovieHub.WebApi.Controllers;

/// <summary>
/// Контроллер для аутентификации пользователей
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    
    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    /// <param name="request">Данные пользователя для регистрации</param>
    /// <returns>Код 200 при успешной регистрации</returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        await _userService.RegisterUserAsync(request);
        return Ok();
    }

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="request">Данные для входа</param>
    /// <returns>JWT токен</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var token = await _userService.LoginAsync(request);
        Response.Cookies.Append("SecurityCookies", token);
        return Ok(token);
    }
    
    /// <summary>
    /// Выход из аккаунта (удаление cookie с токеном)
    /// </summary>
    /// <returns>Код 200 при успешном выходе</returns>
    [HttpPost("logout")]
    [Authorize] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("SecurityCookies");
        return Ok();
    }
        
}