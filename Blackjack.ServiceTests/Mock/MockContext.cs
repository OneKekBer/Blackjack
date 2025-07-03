using Blackjack.Business.Services;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Context;
using Blackjack.Data.Repositories;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.Presentation.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Blackjack.ServiceTests.Mock;

public class MockContext
{
    public IGameService GameService { get; }
    public IPlayerService PlayerService { get; }
    public IGameHubService GameHubService { get; }
    public IDbContextFactory<DatabaseContext> DbContextFactory { get; }
    public IGameRepository GameRepository { get; }
    public IPlayerRepository PlayerRepository { get; }

    public MockContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase("TestInMemoryDatabase")
            .Options;
        
        DbContextFactory = new PooledDbContextFactory<DatabaseContext>(options);
        
        var context = DbContextFactory.CreateDbContext();

        GameRepository = new GameRepository(DbContextFactory);
        PlayerRepository = new PlayerRepository(DbContextFactory);
        PlayerService = new PlayerService(PlayerRepository);
        GameService = new GameService(GameRepository);

        var gameHubDispatcher = new GameHubDispatcher(null, null);
        GameHubService = new GameHubService(PlayerRepository, PlayerService, GameRepository, gameHubDispatcher);
    }
}