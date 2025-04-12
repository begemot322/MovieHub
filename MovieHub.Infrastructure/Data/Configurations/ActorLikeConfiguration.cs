using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieHub.Domain.Entities;

namespace MovieHub.Infrastructure.Data.Configurations;

public class ActorLikeConfiguration : IEntityTypeConfiguration<ActorLike>
{
    public void Configure(EntityTypeBuilder<ActorLike> builder)
    {
        builder.HasKey(al => new { al.ActorId, al.UserId }); 
        
        builder.HasOne(al => al.Actor)
            .WithMany(a => a.ActorLikes)
            .HasForeignKey(al => al.ActorId);
        
        builder.HasOne(al => al.User)
            .WithMany(u => u.ActorLikes)
            .HasForeignKey(al => al.UserId);
    }
}