using Microsoft.EntityFrameworkCore;
using Stations.Data.EntityConfigs;
using Stations.Models;

namespace Stations.Data
{
    public class StationsDbContext : DbContext
    {
        public StationsDbContext()
        {
        }

        public StationsDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<CustomerCard> Cards { get; set; }

        public DbSet<SeatingClass> SeatingClasses { get; set; }

        public DbSet<Station> Stations { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Train> Trains { get; set; }

        public DbSet<TrainSeat> TrainSeats { get; set; }

        public DbSet<Trip> Trips { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionConfig.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SeatingClassConfig());
            modelBuilder.ApplyConfiguration(new StationConfig());
            modelBuilder.ApplyConfiguration(new TicketConfig());
            modelBuilder.ApplyConfiguration(new TrainConfig());
            modelBuilder.ApplyConfiguration(new TrainSeatConfig());
            modelBuilder.ApplyConfiguration(new TripConfig());

            // modelBuilder.Entity<Station>()
            //     .HasAlternateKey(s => s.Name);
               
            // modelBuilder.Entity<Train>()
            //     .HasAlternateKey(t => t.TrainNumber);
               
            // modelBuilder.Entity<SeatingClass>()
            //     .HasAlternateKey(sc => new { sc.Name, sc.Abbreviation });
               
            // modelBuilder.Entity<Trip>()
            //     .HasOne(t => t.DestinationStation)
            //     .WithMany(s => s.TripsTo)
            //     .HasForeignKey(t => t.DestinationStationId);

            // modelBuilder.Entity<Trip>()
            //     .HasOne(t => t.OriginStation)
            //     .WithMany(s => s.TripsFrom)
            //     .HasForeignKey(t => t.OriginStationId)
            //     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}