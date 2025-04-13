using System.Linq.Expressions;
using MovieHub.Application.Common;
using MovieHub.Application.Filters;
using MovieHub.Domain.Entities;

namespace MovieHub.Infrastructure.Extensions;
public static class MovieExtension
{
    public static IQueryable<Movie> Filter(this IQueryable<Movie> query, MovieFilter movieFilter)
    {
        if (!string.IsNullOrEmpty(movieFilter.Title))
            query = query.Where(x => x.Title.Contains(movieFilter.Title));
            
        if (movieFilter.MinLikesCount.HasValue)
            query = query.Where(x => x.MovieLikes.Count() >= movieFilter.MinLikesCount.Value);

        if (movieFilter.MaxLikesCount.HasValue)
            query = query.Where(x => x.MovieLikes.Count() <= movieFilter.MaxLikesCount.Value);

        return query;
    }
    public static IQueryable<Movie> Sort(this IQueryable<Movie> query, SortParams sortParams)
    {
        return sortParams.SortDirection == SortDirection.Descending 
            ? query.OrderByDescending(GetKeySelector(sortParams.OrderBy)) 
            : query.OrderBy(GetKeySelector(sortParams.OrderBy));
    }

    private static Expression<Func<Movie, object>> GetKeySelector(string? sortOrderBy)
    {       
        if (string.IsNullOrEmpty(sortOrderBy))
            return x => x.Title;

        return sortOrderBy switch
        {
            nameof(Movie.ReleaseDate) => x => x.ReleaseDate,
            nameof(Movie.Title) => x => x.Title,
            "LikesCount" => x => x.MovieLikes.Count(),
            _ => x => x.Title
        };
    }
}