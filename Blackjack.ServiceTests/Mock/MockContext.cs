using Blackjack.Business.Services;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Context;
using Blackjack.Data.Interfaces;
using Blackjack.Data.Repositories;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.Presentation.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blackjack.ServiceTests.Mock;

public class MockContext
{
    public IGameService GameService { get; }
    public IPlayerService PlayerService { get; }
    public IGameHubService GameHubService { get; }
    public DatabaseContext DatabaseContext { get; }
    public IGameRepository GameRepository { get; }
    public IPlayerRepository PlayerRepository { get; }

    public MockContext()
    {
        
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase("TestInMemoryDatabase")
            .Options;

        DatabaseContext = new DatabaseContext(options);
        GameRepository = new GameRepository(DatabaseContext);
        PlayerRepository = new PlayerRepository(DatabaseContext);
        PlayerService = new PlayerService(PlayerRepository);
        GameService = new GameService(GameRepository);
        
        var gameHubDispatcher = new GameHubDispatcher(null, null);

        GameHubService = new GameHubService(PlayerRepository, PlayerService, GameRepository, gameHubDispatcher);
    }
}