using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MovieHub.Domain.Entities;

namespace MovieHub.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<User> Users { get; set; }
    
    // Таблицы связи many-to-many
    public DbSet<ActorMovie> ActorMovies { get; set; }
    public DbSet<MovieLike> MovieLikes { get; set; }
    public DbSet<ActorLike> ActorLikes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
    
}