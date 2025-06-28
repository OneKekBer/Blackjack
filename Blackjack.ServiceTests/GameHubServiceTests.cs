using Blackjack.Business.Mappers;
using Blackjack.GameLogic;
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
        Assert.Single(gameEntity!.Players);
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
    
    [Fact]
    public async Task StartGame_WhenGameStarted_GameStateIsUpdated()
    {
        await _mockContext.DatabaseContext.Database.EnsureDeletedAsync();
        await _mockContext.DatabaseContext.Database.EnsureCreatedAsync();
     
        //arrange
        var p1 = new Player(Guid.NewGuid(), "A", Role.User, "", null);
        var p2 = new Player(Guid.NewGuid(), "B", Role.User, "", null);
        var gameId = Guid.NewGuid();
        var gameToDatabase = new Game([p1, p2], gameId);
        var gameEngine = new GameEngine(null, null, null);
        
        //Act
        await _mockContext.GameRepository.Add(GameMapper.ModelToEntity(gameToDatabase));
        _mockContext.DatabaseContext.ChangeTracker.Clear();

        var gameEntity = await _mockContext.GameRepository.GetByIdAsNoTracking(gameId);
        var game = GameMapper.EntityToModel(gameEntity!);
        
        gameEngine.InitGame(game);
        await _mockContext.GameRepository.Update(GameMapper.ModelToEntity(game)!, CancellationToken.None);
        await _mockContext.GameRepository.Save();
        
        var gameEntityFromDatabase = await _mockContext.GameRepository.GetById(gameId);
        
        //Assert
        Assert.Equal(2, gameEntityFromDatabase!.TurnQueue.Count);
    }

    [Fact]
    public async Task AddBotToLobby_AddsBotToLobby_BotsSavesCorrectly()
    {
        await _mockContext.DatabaseContext.Database.EnsureDeletedAsync();
        await _mockContext.DatabaseContext.Database.EnsureCreatedAsync();
        
        //Arrange
        var p1 = new Player(Guid.NewGuid(), "A", Role.User, "", null);
        var p2 = new Player(Guid.NewGuid(), "B", Role.User, "", null);
        var gameId = Guid.NewGuid();
        var gameToDatabase = new Game([p1, p2], gameId);
        var gameEngine = new GameEngine(null, null, null);
        
        //Act
        await _mockContext.GameRepository.Add(GameMapper.ModelToEntity(gameToDatabase));
        _mockContext.DatabaseContext.ChangeTracker.Clear();
        
        var gameEntity = await _mockContext.GameRepository.GetById(gameId);
        var game = GameMapper.EntityToModel(gameEntity!);
        
        var botId = Guid.NewGuid();
        var bot = new Player(botId, $"Bot:{botId}", Role.Bot, "", null);
        var botEntity = PlayerMapper.ModelToEntity(bot);
        
        await _mockContext.PlayerRepository.Add(botEntity, CancellationToken.None);
        gameEntity!.Players.Add(botEntity);

        await _mockContext.GameRepository.Save();
        _mockContext.DatabaseContext.ChangeTracker.Clear();
        
        var gameEntity2 = await _mockContext.GameRepository.GetById(gameId);
        
        //Assert
        Assert.Equal(3, gameEntity2!.Players.Count());
        Assert.Equal(Role.Bot, gameEntity2.Players.First(p => p.Id == botId).Role);
    }

    /*[Fact]
    public async Task GetPlayerAction_WhenPlayerInGame_ReturnsPlayerAction()
    {
        await _mockContext.DatabaseContext.Database.EnsureDeletedAsync();
        await _mockContext.DatabaseContext.Database.EnsureCreatedAsync();

        await _mockContext.GameHubService.GetPlayerAction();
        
        
    }*/
}
