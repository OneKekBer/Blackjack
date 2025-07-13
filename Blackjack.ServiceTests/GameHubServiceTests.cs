using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blackjack.Business.Helpers;
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
        var databaseContext = await _mockContext.InitTest();

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
    }

    [Fact]
    public async Task JoinGame_WhenPlayerAlreadyInGame_DoesNotDuplicatePlayer()
    {
        var databaseContext = await _mockContext.InitTest();

        // Arrange
        var playerId = Guid.NewGuid();
        var gameId = Guid.NewGuid();

        var existingPlayer = new Player(playerId, "Test", Role.User, Guid.NewGuid());
        var game = new Game(new List<Player>(), gameId);

        await _mockContext.PlayerRepository.Add(PlayerMapper.ModelToEntity(existingPlayer));
        await _mockContext.GameRepository.Add(GameMapper.ModelToEntity(game));
        databaseContext.ChangeTracker.Clear();
        // Act
        await _mockContext.GameHubService.JoinGame(playerId, gameId, "connection");

        // Assert
        var gameEntity = await _mockContext.GameRepository.GetById(gameId);
        Assert.Single(gameEntity!.Players);
    }
    
    [Fact]
    public async Task JoinGame_WhenThreeDifferentPlayersJoin_AllAreAddedCorrectly()
    {
        await _mockContext.InitTest();

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
        }
    }
    
    [Fact]
    public async Task StartGameFake_WhenGameStarted_GameStateIsUpdated()
    {
        var databaseContext = await _mockContext.InitTest();

        //arrange
        var p1 = new Player(Guid.NewGuid(), "A", Role.User, null);
        var p2 = new Player(Guid.NewGuid(), "B", Role.User, null);
        var gameId = Guid.NewGuid();
        var gameToDatabase = new Game([p1, p2], gameId);
        var gameEngine = new GameEngine(null, null, null);
        
        //Act
        await _mockContext.GameRepository.Add(GameMapper.ModelToEntity(gameToDatabase));
        databaseContext.ChangeTracker.Clear();

        var gameEntity = await _mockContext.GameRepository.GetById(gameId);
        var game = GameMapper.EntityToModel(gameEntity!);
        
        gameEngine.InitGame(game);
        await _mockContext.GameRepository.Save(GameMapper.ModelToEntity(game));
        
        var gameEntityFromDatabase = await _mockContext.GameRepository.GetById(gameId);
        
        //Assert
        Assert.Equal(2, gameEntityFromDatabase!.TurnQueue.Count);
    }

    [Fact]
    public async Task AddBotToLobby_AddsBotToLobby_BotsSavesCorrectly()
    {
        var databaseContext = await _mockContext.InitTest();
        
        //Arrange
        var p1 = new Player(Guid.NewGuid(), "A", Role.User, null);
        var p2 = new Player(Guid.NewGuid(), "B", Role.User, null);
        var gameId = Guid.NewGuid();
        var gameToDatabase = new Game([p1, p2], gameId);
        var gameEngine = new GameEngine(null, null, null);
        
        //Act
        var entityGameToDatabase = GameMapper.ModelToEntity(gameToDatabase);
        
        await _mockContext.GameRepository.Add(entityGameToDatabase); // players = 2
        databaseContext.ChangeTracker.Clear();
        
        var gameEntity = await _mockContext.GameRepository.GetById(gameId); // players = 0
        var game = GameMapper.EntityToModel(gameEntity!);
        
        var botId = Guid.NewGuid();
        var bot = new Player(botId, $"Bot:{botId}", Role.Bot, null);
        var botEntity = PlayerMapper.ModelToEntity(bot);
        
        //await _mockContext.PlayerRepository.Add(botEntity, CancellationToken.None);
        gameEntity!.Players.Add(botEntity);

        await _mockContext.GameRepository.Save(gameEntity);
        databaseContext.ChangeTracker.Clear();
        
        var gameEntity2 = await _mockContext.GameRepository.GetById(gameId);
        
        //Assert
        Assert.Equal(3, gameEntity2!.Players.Count());
        Assert.Equal(Role.Bot, gameEntity2.Players.First(p => p.Id == botId).Role);
    }
    
    [Fact]
    public async Task StartGame_WhenGameStarted_GameStateIsUpdated()
    {
        var databaseContext = await _mockContext.DbContextFactory.CreateDbContextAsync();

        // Arrange
        await databaseContext.Database.EnsureDeletedAsync();
        await databaseContext.Database.EnsureCreatedAsync();

        var p1 = new Player(Guid.NewGuid(), "A", Role.User, null);
        var p2 = new Player(Guid.NewGuid(), "B", Role.User, null);
        var gameId = Guid.NewGuid();
        var gameToDatabase = new Game([p1, p2], gameId);
        
        /*foreach (var player in gameToDatabase.Players)
        {
            await _mockContext.PlayerRepository.Add(PlayerMapper.ModelToEntity(player));
        }*/
        await _mockContext.GameRepository.Add(GameMapper.ModelToEntity(gameToDatabase));
        databaseContext.ChangeTracker.Clear();

        // Act;
        await _mockContext.GameHubService.StartGame(gameId, CancellationToken.None);
        databaseContext.ChangeTracker.Clear();

        var gameEntityAfterStart = await _mockContext.GameRepository.GetById(gameId);
        var gameAfterStart = GameMapper.EntityToModel(gameEntityAfterStart!);
        // Assert
        Assert.NotNull(gameAfterStart);
        Assert.Equal(2, gameAfterStart.TurnQueue.Count);
        Assert.Contains(gameAfterStart.Players[0].Id, gameAfterStart.TurnQueue);
        Assert.Contains(gameAfterStart.Players[1].Id, gameAfterStart.TurnQueue);
        Assert.Equal(GameStatus.Started, gameAfterStart.Status);
        
        Assert.NotNull(gameEntityAfterStart);
        Assert.Equal(2, gameEntityAfterStart.TurnQueue.Count);
        Assert.Equal(GameStatus.Started, gameEntityAfterStart.Status);
        Assert.Equal(36, CardConverter.StringToCards(gameEntityAfterStart.Deck).Count);
        
    }
}
