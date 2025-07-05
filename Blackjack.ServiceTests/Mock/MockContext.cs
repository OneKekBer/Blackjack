using Blackjack.Business.Services;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Context;
using Blackjack.Data.Repositories;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.Presentation.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Blackjack.ServiceTests.Mock;

public class MockContext
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

    private readonly ILoggerFactory _loggerFactory;

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

        // Логгер фабрика для тестов
        _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .SetMinimumLevel(LogLevel.Debug)
                .AddConsole(); // Можно добавить .AddDebug() или .AddProvider() для тестов
        });

        var gameRepoLogger = _loggerFactory.CreateLogger<GameRepository>();
        var cachedGameRepoLogger = _loggerFactory.CreateLogger<CachedGameRepository>();

        GameRepository = new CachedGameRepository(
            new GameRepository(DbContextFactory, gameRepoLogger),
            Cache,
            cachedGameRepoLogger
        );

        PlayerRepository = new PlayerRepository(DbContextFactory);
        PlayerService = new PlayerService(PlayerRepository);
        GameService = new GameService(GameRepository);

        var gameHubDispatcher = new GameHubDispatcher(null, null);
        GameHubService = new GameHubService(PlayerRepository, PlayerService, GameRepository, gameHubDispatcher);
    }
}
