using Blackjack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blackjack.Data.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }

    public DbSet<GameEntity> Games;
    public DbSet<PlayerEntity> Players;
}