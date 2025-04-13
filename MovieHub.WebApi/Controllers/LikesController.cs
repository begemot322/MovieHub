using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Application.Services.Interfaces;

namespace MovieHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LikesController : ControllerBase
{
    private readonly ILikeService _likeService;
    
    public LikesController(ILikeService likeService)
    {
        _likeService = likeService;
    }
    
    [HttpPost("movies/{movieId:int}")]
    public async Task<IActionResult> LikeMovie(int movieId)
    {
        await _likeService.LikeMovieAsync(movieId);
        return Ok();
    }
    
    [HttpDelete("movies/{movieId:int}")]
    public async Task<IActionResult> UnlikeMovie(int movieId)
    {
        await _likeService.UnlikeMovieAsync(movieId);
        return Ok();
    }
    
    [HttpPost("actors/{actorId:int}")]
    public async Task<IActionResult> LikeActor(int actorId)
    {
        await _likeService.LikeActorAsync(actorId);
        return Ok();
    }
    [HttpDelete("actors/{actorId:int}")]
    public async Task<IActionResult> UnlikeActor(int actorId)
    {
        await _likeService.UnlikeActorAsync(actorId);
        return Ok();
    }
}