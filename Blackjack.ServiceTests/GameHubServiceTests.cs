using Blackjack.Business.Mappers;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;
using Blackjack.ServiceTests.Mock;
using Xunit;

namespace Blackjack.ServiceTests;

public class GameHubServiceTests : IClassFixture<MockContext>
{
    private readonly MockContext _mockContext;

    public GameHubServiceTests(MockContext mockContext)
    {
        _mockContext = mockContext;
    }

    [Fact]
    public async Task JoinGame_WhenPlayerNotInGame_AddsPlayerToGame()
    {
        await _mockContext.DatabaseContext.Database.EnsureDeletedAsync();
        await _mockContext.DatabaseContext.Database.EnsureCreatedAsync();

        // Arrange
        var gameId = Guid.NewGuid();
        var connectionId = "abc123";
        var userId = Guid.NewGuid();
        
        var game = new Game(new List<Player>(), gameId);

        await _mockContext.GameRepository.Add(GameMapper.ModelToEntity(game));

        // Act
        var gameModel = await _mockContext.GameHubService.JoinGame(userId, gameId, connectionId);

        // Assert
        var gameEntity = GameMapper.ModelToEntity(gameModel);
        Assert.Single(gameEntity.Players);
        Assert.Equal(connectionId, gameEntity.Players[0].ConnectionId);
    }

    [Fact]
    public async Task JoinGame_WhenPlayerAlreadyInGame_DoesNotDuplicatePlayer()
    {
        await _mockContext.DatabaseContext.Database.EnsureDeletedAsync();
        await _mockContext.DatabaseContext.Database.EnsureCreatedAsync();

        // Arrange
        var playerId = Guid.NewGuid();
        var connectionId = "abc123";
        var gameId = Guid.NewGuid();

        var existingPlayer = new Player(playerId, "Test", Role.User, connectionId, Guid.NewGuid());
        var game = new Game(new List<Player>(), gameId);

        await _mockContext.PlayerRepository.Add(PlayerMapper.ModelToEntity(existingPlayer));
        await _mockContext.GameRepository.Add(GameMapper.ModelToEntity(game));

        // Act
        var result = await _mockContext.GameHubService.JoinGame(playerId, gameId, connectionId);

        // Assert
        var gameEntity = await _mockContext.GameRepository.GetById(gameId);
        Assert.Single(gameEntity.Players);
    }
    
    [Fact]
    public async Task JoinGame_WhenThreeDifferentPlayersJoin_AllAreAddedCorrectly()
    {
        await _mockContext.DatabaseContext.Database.EnsureDeletedAsync();
        await _mockContext.DatabaseContext.Database.EnsureCreatedAsync();

        // Arrange
        var gameId = Guid.NewGuid();
        var game = new Game(new List<Player>(), gameId);
        await _mockContext.GameRepository.Add(GameMapper.ModelToEntity(game));

        var userIds = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var connectionIds = new[] { "conn1", "conn2", "conn3" };
        
        // Act
        for (int i = 0; i < 3; i++)
        {
             await _mockContext.GameHubService.JoinGame(userIds[i], gameId, connectionIds[i]);
        }

        // Assert
        var gameEntity = GameMapper.EntityToModel(await _mockContext.GameRepository.GetById(gameId));
        Assert.Equal(3, gameEntity.Players.Count);

        foreach (var userId in userIds)
        {
            Assert.Contains(gameEntity.Players, p => p.UserId == userId);
        }

        for (int i = 0; i < 3; i++)
        {
            var player = gameEntity.Players.First(p => p.UserId == userIds[i]);
            Assert.Equal(connectionIds[i], player.ConnectionId);
        }
    }

    /*[Fact]
    public async Task GetPlayerAction_WhenPlayerInGame_ReturnsPlayerAction()
    {
        await _mockContext.DatabaseContext.Database.EnsureDeletedAsync();
        await _mockContext.DatabaseContext.Database.EnsureCreatedAsync();

        await _mockContext.GameHubService.GetPlayerAction();
        
        
    }*/
}
