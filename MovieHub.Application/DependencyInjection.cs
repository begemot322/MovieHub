using Microsoft.Extensions.DependencyInjection;
using MovieHub.Application.Services.Implementation;
using MovieHub.Application.Services.Interfaces;

namespace MovieHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Сервисы
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<ILikeService, LikeService>();

        return services;
    }
}