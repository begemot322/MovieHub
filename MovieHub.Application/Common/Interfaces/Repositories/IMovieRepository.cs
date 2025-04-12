using System.Linq.Expressions;
using MovieHub.Application.Filters;
using MovieHub.Domain.Entities;

namespace MovieHub.Application.Common.Interfaces.Repositories;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAllAsync(MovieFilter? movieFilter = null,
        SortParams? sortParams = null);

    Task<Movie?> GetByIdAsync(int id);

    Task AddAsync(Movie movie);

    void Remove(Movie movie);
    
    Task<bool> ExistsAsync(Expression<Func<Movie, bool>> predicate);

    void Update(Movie movie);
}