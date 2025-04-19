using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieHub.Application.Common.Interfaces.Identity;
using MovieHub.Domain.Entities;

namespace MovieHub.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<DbInitializer>();

        await initialiser.InitialiseAsync();

    }
}

public class DbInitializer
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public DbInitializer(ApplicationDbContext db, IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

   public async Task InitialiseAsync()
    {
        try
        {
            if ((await _db.Database.GetPendingMigrationsAsync()).Any())
            {
                await _db.Database.MigrateAsync();
            }

            // Добавим пользователя, если его ещё нет
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == "moviefan");
            if (user == null)
            {
                user = new User
                {
                    Username = "moviefan",
                    Email = "fan@example.com",
                    PasswordHash = _passwordHasher.Generate("123") // хешируем пароль
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }

            if (!_db.Actors.Any())
            {
                // Добавим актёров
                var actor1 = new Actor
                {
                    Name = "Leonardo",
                    Surname = "DiCaprio",
                    Bio = "American actor and film producer."
                };

                var actor2 = new Actor
                {
                    Name = "Scarlett",
                    Surname = "Johansson",
                    Bio = "American actress and singer."
                };

                _db.Actors.AddRange(actor1, actor2);
                await _db.SaveChangesAsync();

                // Добавим фильмы
                var movie1 = new Movie
                {
                    Title = "Inception",
                    Description = "A mind-bending thriller.",
                    ReleaseDate = new DateTime(2010, 7, 16)
                };

                var movie2 = new Movie
                {
                    Title = "Lucy",
                    Description = "A woman gains extraordinary abilities.",
                    ReleaseDate = new DateTime(2014, 7, 25)
                };

                _db.Movies.AddRange(movie1, movie2);
                await _db.SaveChangesAsync();

                // Связь актёров и фильмов
                _db.ActorMovies.AddRange(
                    new ActorMovie { ActorId = actor1.Id, MovieId = movie1.Id },
                    new ActorMovie { ActorId = actor2.Id, MovieId = movie2.Id }
                );

                // Лайки
                _db.ActorLikes.Add(new ActorLike
                {
                    ActorId = actor1.Id,
                    UserId = user.Id,
                    LikedAt = DateTime.UtcNow
                });

                _db.MovieLikes.Add(new MovieLike
                {
                    MovieId = movie1.Id,
                    UserId = user.Id,
                    LikedAt = DateTime.UtcNow
                });

                await _db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при инициализации базы данных: {ex.Message}");
            throw;
        }
    }
}
    
