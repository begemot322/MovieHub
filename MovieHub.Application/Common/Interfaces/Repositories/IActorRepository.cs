using System.Linq.Expressions;
using MovieHub.Application.Common.QueryParams;
using MovieHub.Domain.Entities;

namespace MovieHub.Application.Common.Interfaces.Repositories;

public interface IActorRepository
{
    Task<IEnumerable<Actor>> GetAllAsync(SortParams? sortParams = null);
    Task<Actor?> GetByIdAsync(int id);
    Task AddAsync(Actor actor);
    void Remove(Actor actor);
    Task<bool> ExistsAsync(Expression<Func<Actor, bool>> predicate);
    Task<List<Actor>> GetByIdsAsync(List<int> ids);
    void Update(Actor actor);
    Task<IEnumerable<Movie>> GetMoviesByActorIdAsync(int id);
}