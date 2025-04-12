using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieHub.Application.Common;
using MovieHub.Application.Common.Interfaces.Repositories;
using MovieHub.Application.Filters;
using MovieHub.Domain.Entities;
using MovieHub.Infrastructure.Extensions;

namespace MovieHub.Infrastructure.Data.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _db;

    public MovieRepository(ApplicationDbContext db)
    {
        _db = db;   
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(MovieFilter? movieFilter = null,
        SortParams? sortParams = null)
    {
        return await _db.Movies
            .Sort(sortParams)
            .Filter(movieFilter)
            .Include(m => m.MovieLikes)
            .Include(m => m.ActorMovies)
                .ThenInclude(am => am.Actor)
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task<Movie?> GetByIdAsync(int id)
    {
        return await _db.Movies
            .Include(m => m.MovieLikes)
            .Include(m => m.ActorMovies)
            .ThenInclude(am => am.Actor)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddAsync(Movie movie)
    {
        await _db.Movies.AddAsync(movie);
    }

    public void Remove(Movie movie)
    {
        _db.Movies.Remove(movie);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Movie, bool>> predicate)
    {
        return await _db.Movies.AnyAsync(predicate);
    }
    
    public void Update(Movie movie)
    {
        _db.Movies.Update(movie);
    }
}