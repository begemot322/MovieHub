using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieHub.Application.Common.Interfaces.Repositories;
using MovieHub.Domain.Entities;

namespace MovieHub.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _db.Users
            .AsNoTracking() 
            .ToListAsync();
    }
    
    public async Task<User?> GetByExpressionAsync(Expression<Func<User, bool>> predicate)
    {
        return await _db.Users
            .Where(predicate)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    public async Task AddAsync(User user)
    {
        await _db.Users.AddAsync(user);
    }

    public void Remove(User user)
    {
        _db.Users.Remove(user);
    }

    public void Update(User user)
    {
        _db.Users.Update(user);
    }
    
    public async Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate)
    {
        return await _db.Users.AnyAsync(predicate);  
    }
    
    public async Task<User?> GetUserWithLikesAsync(int userId)
    {
        return await _db.Users
            .Include(u => u.MovieLikes)
            .Include(u => u.ActorLikes)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
    
}
