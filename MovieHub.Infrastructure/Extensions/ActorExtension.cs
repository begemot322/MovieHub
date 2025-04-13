using System.Linq.Expressions;
using MovieHub.Application.Common;
using MovieHub.Domain.Entities;

namespace MovieHub.Infrastructure.Extensions;

public static class ActorExtension
{
    public static IQueryable<Actor> Sort(this IQueryable<Actor> query, SortParams sortParams)
    {
        return sortParams.SortDirection == SortDirection.Descending 
            ? query.OrderByDescending(GetKeySelector(sortParams.OrderBy)) 
            : query.OrderBy(GetKeySelector(sortParams.OrderBy));
    }

    private static Expression<Func<Actor, object>> GetKeySelector(string? sortOrderBy)
    {       
        if (string.IsNullOrEmpty(sortOrderBy))
            return x => x.Name;

        return sortOrderBy switch
        {
            nameof(Actor.Name) => x => x.Name,
            "LikesCount" => x => x.ActorLikes.Count(),
            _ => x => x.Name
        };
    }
}