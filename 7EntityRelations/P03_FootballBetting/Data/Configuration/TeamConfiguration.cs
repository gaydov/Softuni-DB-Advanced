using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configuration
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.TeamId);

            builder.Property(t => t.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(t => t.LogoUrl)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Initials)
                .IsRequired()
                .HasColumnType("NCHAR(3)");

            builder.HasOne(t => t.PrimaryKitColor)
                .WithMany(c => c.PrimaryKitTeams)
                .HasForeignKey(t => t.PrimaryKitColorId);

            builder.HasOne(t => t.SecondaryKitColor)
                .WithMany(c => c.SecondaryKitTeams)
                .HasForeignKey(t => t.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Town)
                .WithMany(tw => tw.Teams)
                .HasForeignKey(t => t.TownId);
        }
    }
}