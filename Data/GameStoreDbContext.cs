using GameStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Data;

public class GameStoreDbContext : DbContext
{
    public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options) : base(options)
    {
    }

    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<Genre> Genres => Set<Models.Genre>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfigurasi Relasi & Field Game
        modelBuilder.Entity<Game>(entity =>
        {
            entity.Property(g => g.Price).HasColumnType("decimal(18,2)");

            entity.HasOne(g => g.Publisher)
                .WithMany(p => p.Games)
                .HasForeignKey(g => g.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(g => g.Genre)
                .WithMany(gn => gn.Games)
                .HasForeignKey(g => g.GenreId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Konfigurasi Transaction
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(t => t.TotalPrice).HasColumnType("decimal(18,2)");

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