using MovieHub.Application.Common;
using MovieHub.Application.Dtos;
using MovieHub.Domain.Entities;

namespace MovieHub.Application.Services.Interfaces;

public interface IActorService
{
    Task<IEnumerable<Actor>> GetAllAsync(SortParams? sortParams = null);
    Task<Actor> GetByIdAsync(int id);
    Task<Actor> AddActorAsync(ActorDto actorDto);
    Task DeleteActorAsync(int id);
    Task<IEnumerable<Movie>> GetMoviesByActorIdAsync(int actorId);
    Task UpdateActorAsync(int id, ActorDto actorDto);
}