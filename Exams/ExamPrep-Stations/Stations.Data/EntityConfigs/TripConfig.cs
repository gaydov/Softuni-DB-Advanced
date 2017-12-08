using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stations.Models;

namespace Stations.Data.EntityConfigs
{
    public class TripConfig : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasOne(t => t.DestinationStation)
                .WithMany(s => s.TripsTo)
                .HasForeignKey(t => t.DestinationStationId);

            builder.HasOne(t => t.OriginStation)
                .WithMany(s => s.TripsFrom)
                .HasForeignKey(t => t.OriginStationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}