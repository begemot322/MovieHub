namespace MovieHub.Domain.Entities;

public class MovieLike
{
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime LikedAt { get; set; } = DateTime.UtcNow;
}