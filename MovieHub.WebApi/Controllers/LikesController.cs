using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Application.Services.Interfaces;

namespace MovieHub.WebApi.Controllers;

/// <summary>
/// Контроллер для управления лайками фильмов и актёров
/// </summary>
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
    
    /// <summary>
    /// Поставить лайк фильму
    /// </summary>
    /// <param name="movieId">ID фильма</param>
    /// <returns>Код 200 при успешной операции</returns>
    [HttpPost("movies/{movieId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LikeMovie(int movieId)
    {
        await _likeService.LikeMovieAsync(movieId);
        return Ok();
    }
    
    /// <summary>
    /// Убрать лайк с фильма
    /// </summary>
    /// <param name="movieId">ID фильма</param>
    /// <returns>Код 200 при успешной операции</returns>
    [HttpDelete("movies/{movieId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UnlikeMovie(int movieId)
    {
        await _likeService.UnlikeMovieAsync(movieId);
        return Ok();
    }
    
    /// <summary>
    /// Поставить лайк актёру
    /// </summary>
    /// <param name="actorId">ID актёра</param>
    /// <returns>Код 200 при успешной операции</returns>
    [HttpPost("actors/{actorId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LikeActor(int actorId)
    {
        await _likeService.LikeActorAsync(actorId);
        return Ok();
    }
    
    /// <summary>
    /// Убрать лайк с актёра
    /// </summary>
    /// <param name="actorId">ID актёра</param>
    /// <returns>Код 200 при успешной операции</returns>
    [HttpDelete("actors/{actorId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UnlikeActor(int actorId)
    {
        await _likeService.UnlikeActorAsync(actorId);
        return Ok();
    }
}