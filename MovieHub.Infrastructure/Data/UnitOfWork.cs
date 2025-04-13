using MovieHub.Application.Common.Interfaces;
using MovieHub.Application.Common.Interfaces.Repositories;
using MovieHub.Infrastructure.Data.Repositories;

namespace MovieHub.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    
    private IMovieRepository _movieRepository;
    private IActorRepository _actorRepository;
    private IUserRepository _userRepository;


    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public IMovieRepository Movies => 
        _movieRepository ??= new MovieRepository(_db);

    public IActorRepository Actors =>
        _actorRepository ??= new ActorRepository(_db);

    public IUserRepository Users =>
        _userRepository ??= new UserRepository(_db);
    
    
    public void Dispose()
    {
        _db.Dispose();
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync();
    }

}