namespace MovieHub.Domain.Entities;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Description { get; set; }
    
    public ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();
    
    public ICollection<MovieLike> Likes { get; set; } = new List<MovieLike>();

}