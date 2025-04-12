namespace MovieHub.Application.Filters;

public class MovieFilter
{
    public string? Title { get; set; }
    public int? MinLikesCount { get; set; }
    public int? MaxLikesCount { get; set; }
}