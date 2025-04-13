using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieHub.Application.Common;
using MovieHub.Application.Common.Interfaces.Repositories;
using MovieHub.Domain.Entities;
using MovieHub.Infrastructure.Extensions;

namespace MovieHub.Infrastructure.Data.Repositories;

public class ActorRepository : IActorRepository
{
    private readonly ApplicationDbContext _db;

    public ActorRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<IEnumerable<Actor>> GetAllAsync(SortParams? sortParams = null)
    {
        return await _db.Actors
            .Sort(sortParams)
            .Include(a => a.ActorLikes)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Actor?> GetByIdAsync(int id)
    {
        return await _db.Actors
            .Include(a => a.ActorMovies)
                .ThenInclude(am => am.Movie)
            .Include(a => a.ActorLikes)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Actor actor)
    {
        await _db.Actors.AddAsync(actor);
    }

    public void Remove(Actor actor)
    {
        _db.Actors.Remove(actor);
    }
    
    public void Update(Actor actor)
    {
        _db.Actors.Update(actor);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Actor, bool>> predicate)
    {
        return await _db.Actors.AnyAsync(predicate);
    }

    public async Task<List<Actor>> GetByIdsAsync(List<int> ids)
    {
        return await _db.Actors
            .Where(a => ids.Contains(a.Id))
            .ToListAsync();
    }
    

    public async Task<IEnumerable<Movie>> GetMoviesByActorIdAsync(int actorId)
    {
        return await _db.Movies
            .Where(m => m.ActorMovies.Any(am => am.ActorId == actorId))
                .AsNoTracking()  
            .ToListAsync();  
    }
}
