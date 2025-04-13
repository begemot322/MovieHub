using Microsoft.AspNetCore.Mvc;
using MovieHub.Application.Common;
using MovieHub.Application.Dtos;
using MovieHub.Application.Services.Interfaces;
using MovieHub.Domain.Entities;

namespace MovieHub.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActorController : ControllerBase
{
    private readonly IActorService _actorService;

    public ActorController(IActorService actorService)
    {
        _actorService = actorService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Actor>>> GetAll([FromQuery] SortParams? sortParams)
    {
        var actors = await _actorService.GetAllAsync(sortParams);
        return Ok(actors);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Actor>> GetById(int id)
    {
        var actor = await _actorService.GetByIdAsync(id);
        return Ok(actor);
    }
    
    [HttpPost]
    public async Task<ActionResult> AddActor([FromBody] ActorDto actorDto)
    {
        var createdActor =  await _actorService.AddActorAsync(actorDto);
        return CreatedAtAction(nameof(GetById), new { id = createdActor.Id }, createdActor);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _actorService.DeleteActorAsync(id);
        return NoContent();
    }
    
    [HttpGet("{actorId}/movies")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByActorId(int actorId)
    {
        var movies = await _actorService.GetMoviesByActorIdAsync(actorId);
        
        return Ok(movies);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMovie(int id, [FromBody] ActorDto actorDto)
    {
        await _actorService.UpdateActorAsync(id, actorDto);
        return NoContent();
    }
}