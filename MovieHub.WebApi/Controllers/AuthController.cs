using Microsoft.AspNetCore.Mvc;
using MovieHub.Application.Dtos.Authentication;
using MovieHub.Application.Services.Interfaces;

namespace MovieHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    
    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        await _userService.RegisterUserAsync(request);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var token = await _userService.LoginAsync(request);
        Response.Cookies.Append("SecurityCookies", token);
        return Ok(token);
    }
        
}