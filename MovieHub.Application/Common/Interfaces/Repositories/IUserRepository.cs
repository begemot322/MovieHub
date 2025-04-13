using System.Linq.Expressions;
using MovieHub.Domain.Entities;

namespace MovieHub.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();

    Task<User?> GetByExpressionAsync(Expression<Func<User, bool>> predicate);

    Task AddAsync(User user);

    void Remove(User user);

    void Update(User user);

    Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate);

    Task<User?> GetUserWithLikesAsync(int userId);
}