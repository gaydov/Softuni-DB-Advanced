using Instagraph.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagraph.Data.EntityConfigs
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Caption)
                .IsRequired();

            builder.HasOne(po => po.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(po => po.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(po => po.Picture)
                .WithMany(pic => pic.Posts)
                .HasForeignKey(po => po.PictureId);
        }
    }
}