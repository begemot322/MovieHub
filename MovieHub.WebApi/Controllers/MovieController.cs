using Microsoft.AspNetCore.Mvc;
using MovieHub.Application.Common;
using MovieHub.Application.Dtos;
using MovieHub.Application.Filters;
using MovieHub.Application.Services.Interfaces;
using MovieHub.Domain.Entities;

namespace MovieHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetAll([FromQuery] MovieFilter? filter, [FromQuery] SortParams? sortParams)
    {
        var movies = await _movieService.GetAllAsync(filter, sortParams);
        return Ok(movies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetById(int id)
    {
        var movie = await _movieService.GetByIdAsync(id);
        return Ok(movie);
    }

    [HttpPost]
    public async Task<ActionResult> AddMovie([FromBody] MovieDto movieDto)
    {
        var createdMovie = await _movieService.AddMovieAsync(movieDto);

        return CreatedAtAction(nameof(GetById), new { id = createdMovie.Id }, createdMovie);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _movieService.DeleteMovieAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMovie(int id, [FromBody] MovieDto movieDto)
    {
        await _movieService.UpdateMovieAsync(id, movieDto);
        return NoContent();
    }
    
    [HttpGet("{movieId}/actors")]
    public async Task<ActionResult<IEnumerable<Actor>>> GetActorsByMovieId(int movieId)
    {
        var actors = await _movieService.GetActorsByMovieIdAsync(movieId);
        
        return Ok(actors);
    }
}