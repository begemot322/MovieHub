namespace MovieHub.Domain.Entities;

public class Actor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Bio { get; set; }
    
    public ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();
    
    public ICollection<ActorLike> ActorLikes  { get; set; } = new List<ActorLike>();
}