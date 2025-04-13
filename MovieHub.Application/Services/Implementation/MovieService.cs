using System.ComponentModel.DataAnnotations;
using MovieHub.Application.Common;
using MovieHub.Application.Common.Exceptions;
using MovieHub.Application.Common.Interfaces;
using MovieHub.Application.Common.Interfaces.Repositories;
using MovieHub.Application.Dtos;
using MovieHub.Application.Filters;
using MovieHub.Application.Services.Interfaces;
using MovieHub.Domain.Entities;

namespace MovieHub.Application.Services.Implementation;

public class MovieService : IMovieService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public MovieService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(MovieFilter? filter = null, SortParams? sortParams = null)
    {
        var movies = await _unitOfWork.Movies.GetAllAsync(filter, sortParams);

        if (!movies.Any())
            throw new NotFoundException("Фильмы не найдены");

        return movies;
    }
    
    public async Task<IEnumerable<Actor>> GetActorsByMovieIdAsync(int movieId)
    {
        var movie = await _unitOfWork.Movies.GetByIdAsync(movieId);

        if (movie == null)
            throw new NotFoundException($"Фильм с Id {movieId} не найден");
        
        var movieActors = await _unitOfWork.Movies.GetActorsByMovieIdAsync(movieId); 
    
        if (!movieActors.Any())
            throw new NotFoundException($"Фильм с Id {movieId} не найден или в нем нет актеров");

        return movieActors;
    }


    public async Task<Movie> GetByIdAsync(int id)
    {
        var movie = await _unitOfWork.Movies.GetByIdAsync(id);

        if (movie == null)
            throw new NotFoundException($"Фильм с Id {id} не найден");

        return movie;
    }

    public async Task<Movie> AddMovieAsync(MovieDto movieDto)
    {
        if (string.IsNullOrWhiteSpace(movieDto.Title))
            throw new ValidationException("Название фильма не может быть пустым");
        
        var exists = await _unitOfWork.Movies.ExistsAsync(m => m.Title == movieDto.Title);
        if (exists)
            throw new DuplicateException($"Фильм с названием {movieDto.Title} уже существует");
        
        var movie = new Movie
        {
            Title = movieDto.Title,
            ReleaseDate = movieDto.ReleaseDate,
            Description = movieDto.Description,
        };
        
        var actors = await _unitOfWork.Actors.GetByIdsAsync(movieDto.ActorIds);
        
        if (actors.Count != movieDto.ActorIds.Count)
            throw new NotFoundException("Один или несколько актеров не найдены");
        
        foreach (var actor in actors)
        {
            movie.ActorMovies.Add(new ActorMovie
            {
                Actor = actor,
                Movie = movie
            });
        }
        await _unitOfWork.Movies.AddAsync(movie);
        await _unitOfWork.SaveChangesAsync(); 
        
        return movie;
    }

    public async Task DeleteMovieAsync(int id)
    {
        var movie = await _unitOfWork.Movies.GetByIdAsync(id);

        if (movie == null)
            throw new NotFoundException($"Фильм с Id {id} не найден");

        _unitOfWork.Movies.Remove(movie);
        await _unitOfWork.SaveChangesAsync(); 
    }

    public async Task UpdateMovieAsync(int id, MovieDto movieDto)
    {
        if (string.IsNullOrWhiteSpace(movieDto.Title))
            throw new ValidationException("Название фильма не может быть пустым");
        
        var movie = await _unitOfWork.Movies.GetByIdAsync(id);
        if (movie == null)
            throw new NotFoundException($"Фильм с Id {id} не найден");
        
        var exists = await _unitOfWork.Movies.ExistsAsync(m => m.Title == movieDto.Title && m.Id != id);
        if (exists)
            throw new DuplicateException($"Фильм с названием {movieDto.Title} уже существует");
        
        movie.Title = movieDto.Title;
        movie.ReleaseDate = movieDto.ReleaseDate;
        movie.Description = movieDto.Description;
        
        var actors = await _unitOfWork.Actors.GetByIdsAsync(movieDto.ActorIds);
        
        if (actors.Count != movieDto.ActorIds.Count)
            throw new NotFoundException("Один или несколько актеров не найдены");
        
        movie.ActorMovies.Clear();
        
        foreach (var actor in actors)
        {
            movie.ActorMovies.Add(new ActorMovie
            {
                ActorId = actor.Id,
                MovieId = movie.Id
            });
        }
        
        await _unitOfWork.SaveChangesAsync();
    }
}