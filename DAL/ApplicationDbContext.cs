using Microsoft.EntityFrameworkCore;
using DLL.Entities;

namespace DAL;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Drink> Drinks { get; set; } = null!;
    public DbSet<DrinkType> DrinkTypes { get; set; } = null!;
    public DbSet<Admin> Admins { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<Rule> Rules { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Drink>()
            .HasOne(d => d.Player)
            .WithMany(p => p.Drinks)
            .HasForeignKey(d => d.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Drink>()
            .HasOne(d => d.DrinkType)
            .WithMany()
            .HasForeignKey(d => d.DrinkTypeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Player>()
            .HasOne(p => p.Game)
            .WithMany(g => g.Players)
            .HasForeignKey(p => p.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Game>()
            .HasMany(g => g.Players)
            .WithOne(p => p.Game)
            .HasForeignKey(p => p.GameId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<DrinkType>().HasData(
            new DrinkType { Id = 1, Name = "Required" },
            new DrinkType { Id = 2, Name = "GrotemannenBak" },
            new DrinkType { Id = 3, Name = "Solidariteit" }
        );
    }
}