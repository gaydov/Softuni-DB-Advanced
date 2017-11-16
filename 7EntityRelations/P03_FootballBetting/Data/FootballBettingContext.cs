using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {
        }

        public FootballBettingContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionConfig.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bet>(entity =>
            {
                entity.HasKey(b => b.BetId);

                entity.Property(b => b.DateTime)
                    .IsRequired();

                entity.HasOne(b => b.Game)
                    .WithMany(g => g.Bets)
                    .HasForeignKey(b => b.GameId);

                entity.HasOne(b => b.User)
                    .WithMany(g => g.Bets)
                    .HasForeignKey(b => b.UserId);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.HasKey(c => c.ColorId);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(ct => ct.CountryId);

                entity.Property(ct => ct.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.GameId);

                entity.Property(g => g.DateTime)
                    .IsRequired();

                entity.Property(g => g.Result)
                    .IsRequired();

                entity.HasOne(g => g.HomeTeam)
                    .WithMany(t => t.HomeGames)
                    .HasForeignKey(g => g.HomeTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(g => g.AwayTeam)
                    .WithMany(t => t.AwayGames)
                    .HasForeignKey(g => g.AwayTeamId);
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(p => p.PlayerId);

                entity.Property(p => p.Name)
                    .IsRequired();

                entity.HasOne(p => p.Team)
                    .WithMany(t => t.Players)
                    .HasForeignKey(p => p.TeamId);

                entity.HasOne(pl => pl.Position)
                    .WithMany(ps => ps.Players)
                    .HasForeignKey(pl => pl.PositionId);
            });

            modelBuilder.Entity<PlayerStatistic>(entity =>
            {
                entity.HasKey(ps => new { ps.PlayerId, ps.GameId });

                entity.HasOne(ps => ps.Player)
                    .WithMany(p => p.PlayerStatistics)
                    .HasForeignKey(ps => ps.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ps => ps.Game)
                    .WithMany(p => p.PlayerStatistics)
                    .HasForeignKey(ps => ps.GameId);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(p => p.PositionId);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.TeamId);

                entity.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(t => t.LogoUrl)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(t => t.Initials)
                    .IsRequired()
                    .HasColumnType("CHAR(3)");

                entity.HasOne(t => t.PrimaryKitColor)
                    .WithMany(c => c.PrimaryKitTeams)
                    .HasForeignKey(t => t.PrimaryKitColorId);

                entity.HasOne(t => t.SecondaryKitColor)
                    .WithMany(c => c.SecondaryKitTeams)
                    .HasForeignKey(t => t.SecondaryKitColorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Town)
                    .WithMany(tw => tw.Teams)
                    .HasForeignKey(t => t.TownId);
            });

            modelBuilder.Entity<Town>(entity =>
            {
                entity.HasKey(tw => tw.TownId);

                entity.Property(tw => tw.Name)
                    .IsRequired();

                entity.HasOne(tw => tw.Country)
                    .WithMany(c => c.Towns)
                    .HasForeignKey(tw => tw.CountryId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);

                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}