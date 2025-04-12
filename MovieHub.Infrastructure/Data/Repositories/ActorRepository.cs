using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieHub.Application.Common.Interfaces.Repositories;
using MovieHub.Domain.Entities;

namespace MovieHub.Infrastructure.Data.Repositories;

public class ActorRepository : IActorRepository
{
    private readonly ApplicationDbContext _db;

    public ActorRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<IEnumerable<Actor>> GetAllAsync()
    {
        return await _db.Actors
            .Include(a => a.ActorMovies)
                .ThenInclude(am => am.Movie)
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
}
