using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieHub.Domain.Entities;

namespace MovieHub.Infrastructure.Data.Configurations;

public class MovieLikeConfiguration : IEntityTypeConfiguration<MovieLike>
{
    public void Configure(EntityTypeBuilder<MovieLike> builder)
    {
        builder.HasKey(ml => new { ml.MovieId, ml.UserId });

        builder.HasOne(ml => ml.Movie)
            .WithMany(m => m.MovieLikes)
            .HasForeignKey(ml => ml.MovieId);

        builder.HasOne(ml => ml.User)
            .WithMany(u => u.MovieLikes)
            .HasForeignKey(ml => ml.UserId);
    }
}