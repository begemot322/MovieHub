using Microsoft.AspNetCore.Mvc;
using MovieHub.Application.Common;
using MovieHub.Application.Common.QueryParams;
using MovieHub.Application.Dtos;
using MovieHub.Application.Services.Interfaces;
using MovieHub.Domain.Entities;

namespace MovieHub.WebApi.Controllers;

/// <summary>
/// Контроллер для работы с актерами
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ActorController : ControllerBase
{
    private readonly IActorService _actorService;

    public ActorController(IActorService actorService)
    {
        _actorService = actorService;
    }
    
    /// <summary>
    /// Получение списка всех актеров с возможностью сортировки
    /// </summary>
    /// <param name="sortParams">Параметры сортировки (необязательный)</param>
    /// <returns>Список актеров</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Actor>))]
    public async Task<ActionResult<IEnumerable<Actor>>> GetAll([FromQuery] SortParams? sortParams)
    {
        var actors = await _actorService.GetAllAsync(sortParams);
        return Ok(actors);
    }
    
    /// <summary>
    /// Получение актера по ID
    /// </summary>
    /// <param name="id">Идентификатор актера</param>
    /// <returns>Возвращает найденного актера</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Actor))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Actor>> GetById(int id)
    {
        var actor = await _actorService.GetByIdAsync(id);
        return Ok(actor);
    }
    

    /// <summary>
    /// Добавление нового актера
    /// </summary>
    /// <param name="actorDto">DTO с данными актера</param>
    /// <returns>Id нового актера</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddActor([FromBody] ActorDto actorDto)
    {
        var createdActor =  await _actorService.AddActorAsync(actorDto);
        return CreatedAtAction(nameof(GetById), new { id = createdActor.Id }, createdActor);
    }
    
    /// <summary>
    /// Удаление актера по ID
    /// </summary>
    /// <param name="id">Идентификатор актера</param>
    /// <returns>void</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _actorService.DeleteActorAsync(id);
        return NoContent();
    }
    
    /// <summary>
    /// Получение списка фильмов, в которых участвовал актер
    /// </summary>
    /// <param name="actorId">Идентификатор актера</param>
    /// <returns>Возвращает список фильмов</returns>
    [HttpGet("{actorId}/movies")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Movie>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByActorId(int actorId)
    {
        var movies = await _actorService.GetMoviesByActorIdAsync(actorId);
        
        return Ok(movies);
    }
    
    /// <summary>
    /// Обновление данных актера
    /// </summary>
    /// <param name="id">Идентификатор актера</param>
    /// <param name="actorDto">DTO с обновленными данными</param>
    /// <returns>Успешное обновление</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateActor(int id, [FromBody] ActorDto actorDto)
    {
        await _actorService.UpdateActorAsync(id, actorDto);
        return NoContent();
    }
}