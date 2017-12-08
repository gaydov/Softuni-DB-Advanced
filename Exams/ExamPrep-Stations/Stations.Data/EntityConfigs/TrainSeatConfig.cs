using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stations.Models;

namespace Stations.Data.EntityConfigs
{
    public class TrainSeatConfig : IEntityTypeConfiguration<TrainSeat>
    {
        public void Configure(EntityTypeBuilder<TrainSeat> builder)
        {
            builder.HasOne(ts => ts.Train)
                .WithMany(t => t.TrainSeats)
                .HasForeignKey(ts => ts.TrainId);
        }
    }
}