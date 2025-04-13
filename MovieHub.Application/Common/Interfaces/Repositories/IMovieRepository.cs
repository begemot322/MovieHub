using System.Linq.Expressions;
using MovieHub.Application.Common.QueryParams;
using MovieHub.Application.Filters;
using MovieHub.Domain.Entities;

namespace MovieHub.Application.Common.Interfaces.Repositories;

public interface IMovieRepository
{
    Task<PagedResult<Movie>> GetAllAsync(MovieFilter? movieFilter = null,
        SortParams? sortParams = null, PageParams? pageParams = null);

    Task<Movie?> GetByIdAsync(int id);

    Task AddAsync(Movie movie);

    void Remove(Movie movie);
    
    Task<bool> ExistsAsync(Expression<Func<Movie, bool>> predicate);

    void Update(Movie movie);
    Task<IEnumerable<Actor>> GetActorsByMovieIdAsync(int movieId);
}