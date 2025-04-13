using MovieHub.Application.Common.Exceptions;
using MovieHub.Application.Common.Interfaces;
using MovieHub.Application.Services.Interfaces;
using MovieHub.Domain.Entities;

namespace MovieHub.Application.Services.Implementation;

public class LikeService : ILikeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    
    public LikeService(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task LikeMovieAsync(int movieId)
    {
        var user = await GetUserWithLikesAsync();
    
        if (user.MovieLikes.Any(ml => ml.MovieId == movieId))
            throw new DuplicateException("Вы уже лайкнули этот фильм");

        var movie = await _unitOfWork.Movies.GetByIdAsync(movieId);
        if (movie == null)
            throw new NotFoundException($"Фильм с Id {movieId} не найден");

        user.MovieLikes.Add(new MovieLike { MovieId = movieId, UserId = user.Id, LikedAt = DateTime.UtcNow });
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task UnlikeMovieAsync(int movieId)
    {
        var user = await GetUserWithLikesAsync();
        
        var movieLike = user.MovieLikes.FirstOrDefault(ml => ml.MovieId == movieId);
        if (movieLike == null)
            throw new NotFoundException("Лайк для этого фильма не найден");

        user.MovieLikes.Remove(movieLike);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task LikeActorAsync(int actorId)
    {
        var user = await GetUserWithLikesAsync();
    
        if (user.ActorLikes.Any(al => al.ActorId == actorId))
            throw new DuplicateException("Вы уже лайкнули этого актера");

        var actor = await _unitOfWork.Actors.GetByIdAsync(actorId);
        if (actor == null)
            throw new NotFoundException($"Актер с Id {actorId} не найден");

        user.ActorLikes.Add(new ActorLike { ActorId = actorId, UserId = user.Id, LikedAt = DateTime.UtcNow });
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task UnlikeActorAsync(int actorId)
    {
        var user = await GetUserWithLikesAsync();
        
        var actorLike = user.ActorLikes.FirstOrDefault(al => al.ActorId == actorId);
        if (actorLike == null)
            throw new NotFoundException("Лайк для этого актера не найден");

        user.ActorLikes.Remove(actorLike);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<User> GetUserWithLikesAsync()
    {
        var userId = _userContext.GetCurrentUserId();
        if (userId == null)
            throw new UnauthorizedAccessException("Пользователь не авторизован");

        var user = await _unitOfWork.Users.GetUserWithLikesAsync(userId.Value);
        if (user == null)
            throw new NotFoundException($"Пользователь с Id {userId} не найден");

        return user;
    }
}