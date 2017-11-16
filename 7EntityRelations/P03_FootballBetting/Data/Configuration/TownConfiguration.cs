using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configuration
{
    public class TownConfiguration : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> builder)
        {
            builder.HasKey(tw => tw.TownId);

            builder.Property(tw => tw.Name)
                .IsRequired();

            builder.HasOne(tw => tw.Country)
                .WithMany(c => c.Towns)
                .HasForeignKey(tw => tw.CountryId);
        }
    }
}