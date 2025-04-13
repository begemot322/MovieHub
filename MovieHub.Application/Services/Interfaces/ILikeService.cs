namespace MovieHub.Application.Services.Interfaces;

public interface ILikeService
{
    Task LikeMovieAsync(int movieId);
    Task LikeActorAsync(int actorId);
}