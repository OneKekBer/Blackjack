using Blackjack.Business.Services;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Context;
using Blackjack.Data.Repositories;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.Presentation.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;

namespace Blackjack.ServiceTests.Mock;

public class MockContext // why mockcontext can init and clear cache?
                         // idk, its just handy, but if i will have more functionality i will separate logic(i hope)
{
    public IGameService GameService { get; }
    public IMemoryCache Cache { get; set; } = new MemoryCache(new MemoryCacheOptions()
    {
        SizeLimit = 1024
    });
    public IPlayerService PlayerService { get; }
    public IGameHubService GameHubService { get; }
    public IDbContextFactory<DatabaseContext> DbContextFactory { get; }
    public IGameRepository GameRepository { get; }
    public IPlayerRepository PlayerRepository { get; }

    public void ClearCache()
    {
        Cache = new MemoryCache(new MemoryCacheOptions()
        {
            SizeLimit = 1024
        });
    }

    public async Task<DatabaseContext> InitTest()
    {
        var databaseContext = await DbContextFactory.CreateDbContextAsync();
        ClearCache();
        await databaseContext.Database.EnsureDeletedAsync();
        await databaseContext.Database.EnsureCreatedAsync();
        
        return databaseContext;
    }
    
    public MockContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase("TestInMemoryDatabase")
            .Options;
        
        DbContextFactory = new PooledDbContextFactory<DatabaseContext>(options);
        
        var context = DbContextFactory.CreateDbContext();
        
        GameRepository = new CachedGameRepository(new GameRepository(DbContextFactory), Cache, null);
        PlayerRepository = new PlayerRepository(DbContextFactory);
        PlayerService = new PlayerService(PlayerRepository);
        GameService = new GameService(GameRepository);

        var gameHubDispatcher = new GameHubDispatcher(null, null);
        GameHubService = new GameHubService(PlayerRepository, PlayerService, GameRepository, gameHubDispatcher);
    }
}