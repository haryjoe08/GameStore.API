using GameStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Data;

public class GameStoreDbContext : DbContext
{
    public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<Genre> Genres => Set<Models.Genre>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Konfigurasi Entity Genre
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Name).IsRequired().HasMaxLength(100);
        });

        // 2. Konfigurasi Entity Publisher
        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
        });

        // 3. Konfigurasi Entity Game
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Title).IsRequired().HasMaxLength(200);
            entity.Property(g => g.Price).HasPrecision(18, 2);

            // Relasi Game -> Genre (1-to-N)
            entity.HasOne(g => g.Genre)
                .WithMany(gn => gn.Games)
                .HasForeignKey(g => g.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relasi Game -> Publisher (1-to-N)
            entity.HasOne(g => g.Publisher)
                .WithMany(p => p.Games)
                .HasForeignKey(g => g.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // 4. Konfigurasi Entity Transaction
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.TotalPrice).HasPrecision(18, 2);

            // Relasi Transaction -> Game (1-to-N)
            entity.HasOne(t => t.Game)
                .WithMany(g => g.Transactions)
                .HasForeignKey(t => t.GameId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed Initial Data (Data Awal)
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Publisher>().HasData(
            new Publisher { Id = 1, Name = "Rockstar Games", Country = "USA" },
            new Publisher { Id = 2, Name = "Sony Interactive", Country = "Japan" }
        );

        modelBuilder.Entity<Models.Genre>().HasData(
            new Models.Genre { Id = 1, Name = "Action-Adventure" },
            new Models.Genre { Id = 2, Name = "RPG" }
        );

        modelBuilder.Entity<Game>().HasData(
            new Game
            {
                Id = 1, Title = "Red Dead Redemption 2", Price = 59.99m, Stock = 20, PublisherId = 1, GenreId = 1
            },
            new Game
            {
                Id = 2, Title = "The Last of Us Part I", Price = 69.99m, Stock = 15, PublisherId = 2, GenreId = 1
            }
        );
    }
}