using System.ComponentModel.DataAnnotations;
using MovieHub.Application.Common;
using MovieHub.Application.Common.Exceptions;
using MovieHub.Application.Common.QueryParams;
using MovieHub.Application.Dtos;
using MovieHub.Application.Filters;
using MovieHub.Domain.Entities;

namespace MovieHub.Application.Services.Interfaces;

public interface IMovieService
{
    Task<PagedResult<Movie>> GetAllAsync(MovieFilter? filter = null,
        SortParams? sortParams = null, PageParams? pageParams = null);
    Task<Movie> GetByIdAsync(int id);

    Task<Movie> AddMovieAsync(MovieDto movieDto);

    Task DeleteMovieAsync(int id);

    Task UpdateMovieAsync(int id, MovieDto movieDto);
    Task<IEnumerable<Actor>> GetActorsByMovieIdAsync(int movieId);

}