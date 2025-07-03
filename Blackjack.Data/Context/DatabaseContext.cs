using Blackjack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blackjack.Data.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext() { }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) 
        : base(options) {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<GameEntity> Games { get; set; }
    public DbSet<PlayerEntity> Players { get; set; }
}