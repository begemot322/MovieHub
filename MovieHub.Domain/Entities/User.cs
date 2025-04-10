namespace MovieHub.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    public ICollection<ActorLike> ActorLikes { get; set; } = new List<ActorLike>();

    public ICollection<MovieLike> MovieLikes { get; set; } = new List<MovieLike>();
}