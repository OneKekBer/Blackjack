using Blackjack.Business.Mappers;
using Blackjack.GameLogic;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;
using Blackjack.Presentation.Hubs;
using Blackjack.ServiceTests.Mock;
using Xunit;

namespace Blackjack.ServiceTests;

public class GameHubDispatcherServiceTests : IClassFixture<MockContext>
{
    private readonly MockContext _mockContext;

    public GameHubDispatcherServiceTests(MockContext mockContext)
    {
        _mockContext = mockContext;
    }

    [Fact]
    public async Task SaveGame_WhenSaveGame_DataSavesCorrectly()
    {
        var databaseContext = await _mockContext.DbContextFactory.CreateDbContextAsync();
        // Arrange
        await databaseContext.Database.EnsureDeletedAsync();
        await databaseContext.Database.EnsureCreatedAsync();

        var p1 = new Player(Guid.NewGuid(), "A", Role.User, "", null);
        var p2 = new Player(Guid.NewGuid(), "B", Role.User, "", null);
        var gameId = Guid.NewGuid();
        var game = new Game([p1, p2], gameId);
    
        var gameEngine = new GameEngine(null, null, null);
        gameEngine.InitGame(game);

        var cardA = new Card(Suits.Diamond, Rank.Queen);
        var cardB = new Card(Suits.Diamond, Rank.Jack);

        game.Players.First(p => p.Name == "A").Cards.Add(cardA);
        game.Players.First(p => p.Name == "B").Cards.Add(cardB);
        
        await _mockContext.GameRepository.Add(GameMapper.ModelToEntity(game));
        await _mockContext.GameRepository.Save();
        databaseContext.ChangeTracker.Clear();
        
        game.Players.First(p => p.Name == "A").Balance = 1500;

        var dispatcher = new GameHubDispatcher(null!, null); 
        await dispatcher.SaveGame(game);
        databaseContext.ChangeTracker.Clear();

        // Act
        var updatedGameEntity = await _mockContext.GameRepository.GetById(gameId);
        var updatedGame = GameMapper.EntityToModel(updatedGameEntity!);

        var updatedPlayerA = updatedGame.Players.First(p => p.Name == "A");
        var updatedPlayerB = updatedGame.Players.First(p => p.Name == "B");

        // Assert
        Assert.Single(updatedPlayerA.Cards);
        Assert.Equal(cardA, updatedPlayerA.Cards.First());
        Assert.Single(updatedPlayerB.Cards);
        Assert.Equal(cardB, updatedPlayerB.Cards.First());
        Assert.Equal(1500, updatedPlayerA.Balance);
    }

}