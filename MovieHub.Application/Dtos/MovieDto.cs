namespace MovieHub.Application.Dtos;

public class MovieDto
{
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Description { get; set; }
    public List<int> ActorIds { get; set; } = new();
}