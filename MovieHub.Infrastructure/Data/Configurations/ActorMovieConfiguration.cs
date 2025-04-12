using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieHub.Domain.Entities;

namespace MovieHub.Infrastructure.Data.Configurations;

public class ActorMovieConfiguration : IEntityTypeConfiguration<ActorMovie>
{
    public void Configure(EntityTypeBuilder<ActorMovie> builder)
    {
        builder.HasKey(am => new { am.ActorId, am.MovieId });

        builder.HasOne(am => am.Actor)
            .WithMany(a => a.ActorMovies)
            .HasForeignKey(am => am.ActorId);

        builder.HasOne(am => am.Movie)
            .WithMany(m => m.ActorMovies)
            .HasForeignKey(am => am.MovieId);
    }
}