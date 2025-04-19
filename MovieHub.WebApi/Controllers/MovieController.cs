using Microsoft.AspNetCore.Mvc;
using MovieHub.Application.Common;
using MovieHub.Application.Common.QueryParams;
using MovieHub.Application.Dtos;
using MovieHub.Application.Filters;
using MovieHub.Application.Services.Interfaces;
using MovieHub.Domain.Entities;

namespace MovieHub.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с фильмами
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    /// <summary>
    /// Получение списка фильмов с фильтрацией, сортировкой и пагинацией
    /// </summary>
    /// <param name="filter">Фильтрация фильмов по различным параметрам (необязательно)</param>
    /// <param name="sortParams">Параметры сортировки (необязательно)</param>
    /// <param name="pageParams">Параметры пагинации (необязательно)</param>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Movie>))]
    public async Task<ActionResult<IEnumerable<Movie>>> GetAll([FromQuery] MovieFilter? filter,
        [FromQuery] SortParams? sortParams,  [FromQuery] PageParams pageParams)
    {
        var movies = await _movieService.GetAllAsync(filter, sortParams,pageParams);
        return Ok(movies);
    }

    /// <summary>
    /// Получение фильма по ID
    /// </summary>
    /// <param name="id">Идентификатор фильма</param>
    /// <returns>Найденный фильм</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Movie))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Movie>> GetById(int id)
    {
        var movie = await _movieService.GetByIdAsync(id);
        return Ok(movie);
    }

    /// <summary>
    /// Добавление нового фильма
    /// </summary>
    /// <param name="movieDto">DTO с данными фильма</param>
    /// <returns>Id нового фильма</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Movie))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddMovie([FromBody] MovieDto movieDto)
    {
        var createdMovie = await _movieService.AddMovieAsync(movieDto);

        return CreatedAtAction(nameof(GetById), new { id = createdMovie.Id }, createdMovie);
    }

    /// <summary>
    /// Удаление фильма по ID
    /// </summary>
    /// <param name="id">Идентификатор фильма</param>
    /// <returns>Код 204 при успешном удалении</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _movieService.DeleteMovieAsync(id);
        return NoContent();
    }
    
    /// <summary>
    /// Обновление данных фильма
    /// </summary>
    /// <param name="id">Идентификатор фильма</param>
    /// <param name="movieDto">DTO с обновленными данными</param>
    /// <returns>Код 204 при успешном обновлении</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateMovie(int id, [FromBody] MovieDto movieDto)
    {
        await _movieService.UpdateMovieAsync(id, movieDto);
        return NoContent();
    }
    
    /// <summary>
    /// Получение списка актеров, снявшихся в фильме
    /// </summary>
    /// <param name="movieId">Идентификатор фильма</param>
    /// <returns>Список актеров</returns>
    [HttpGet("{movieId}/actors")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Actor>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Actor>>> GetActorsByMovieId(int movieId)
    {
        var actors = await _movieService.GetActorsByMovieIdAsync(movieId);
        
        return Ok(actors);
    }
}