using System.ComponentModel.DataAnnotations;
using MovieHub.Application.Common;
using MovieHub.Application.Common.Exceptions;
using MovieHub.Application.Common.Interfaces;
using MovieHub.Application.Dtos;
using MovieHub.Application.Services.Interfaces;
using MovieHub.Domain.Entities;

namespace MovieHub.Application.Services.Implementation;

public class ActorService : IActorService
{
    private readonly IUnitOfWork _unitOfWork;

    public ActorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<Actor>> GetAllAsync(SortParams? sortParams = null)
    {
        var actors = await _unitOfWork.Actors.GetAllAsync(sortParams);
        
        if (!actors.Any())
            throw new NotFoundException("Актёры не найдены");

        return actors;
    }
    
    public async Task<Actor> GetByIdAsync(int id)
    {
        var actor = await _unitOfWork.Actors.GetByIdAsync(id);

        if (actor == null)
            throw new NotFoundException($"Актёр с Id {id} не найден");

        return actor;
    }
    
    public async Task<Actor> AddActorAsync(ActorDto actorDto)
    {
        if (string.IsNullOrWhiteSpace(actorDto.Name) || string.IsNullOrWhiteSpace(actorDto.Surname))
            throw new ValidationException("Имя и фамилия актёра не могут быть пустыми");

        var exists = await _unitOfWork.Actors.ExistsAsync(a =>
            a.Name == actorDto.Name && a.Surname== actorDto.Surname);
        if (exists)
            throw new DuplicateException($"Актёр с именем {actorDto.Name} и фамилией {actorDto.Surname} уже существует");

        var actor = new Actor()
        {
            Name = actorDto.Name,
            Surname = actorDto.Surname,
            Bio = actorDto.Bio
        };

        await _unitOfWork.Actors.AddAsync(actor);
        await _unitOfWork.SaveChangesAsync();

        return actor;
    }
    
    public async Task DeleteActorAsync(int id)
    {
        var actor = await _unitOfWork.Actors.GetByIdAsync(id);

        if (actor == null)
            throw new NotFoundException($"Актёр с Id {id} не найден");

        _unitOfWork.Actors.Remove(actor);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<IEnumerable<Movie>> GetMoviesByActorIdAsync(int actorId)
    {
        var actor = await _unitOfWork.Actors.GetByIdAsync(actorId);

        if (actor == null)
            throw new NotFoundException($"Актёр с Id {actorId} не найден");
        
        var actorMovies = await _unitOfWork.Actors.GetMoviesByActorIdAsync(actorId); 
    
        if (!actorMovies.Any())
            throw new NotFoundException($"Актер с Id {actorId} не найден или не участвует в фильмах");

        return actorMovies;
    }

    public async Task UpdateActorAsync(int id, ActorDto actorDto)
    {
        if (string.IsNullOrWhiteSpace(actorDto.Name) || string.IsNullOrWhiteSpace(actorDto.Surname))
            throw new ValidationException("Имя и фамилия актёра не могут быть пустыми");
        
        var actor = await _unitOfWork.Actors.GetByIdAsync(id);

        if (actor == null)
            throw new NotFoundException($"Актёр с Id {id} не найден");
        
        var exists = await _unitOfWork.Actors.ExistsAsync(a =>
            a.Name == actorDto.Name && a.Surname == actorDto.Surname && a.Id != id);
        if (exists)
            throw new DuplicateException($"Актёр с именем {actorDto.Name} и фамилией {actorDto.Surname} уже существует");

        actor.Name = actorDto.Name;
        actor.Surname = actorDto.Surname;
        actor.Bio = actorDto.Bio;
        
        await _unitOfWork.SaveChangesAsync();
    }
}