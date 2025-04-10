namespace MovieHub.Domain.Entities;

public class ActorLike
{
    public int ActorId { get; set; }
    public Actor Actor { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime LikedAt { get; set; } = DateTime.UtcNow;
}