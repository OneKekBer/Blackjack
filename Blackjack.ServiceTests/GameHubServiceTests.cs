using Blackjack.Business.Mappers;
using Blackjack.Business.Services;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Context;
using Blackjack.Data.Repositories;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Blackjack.ServiceTests;

public class GameHubServiceTests
{
    private readonly IGameService _gameService;
    private readonly IPlayerService _playerService;
    private readonly IGameHubService _gameHubService;
    private readonly DatabaseContext _context;
    private readonly IGameRepository _gameRepository;
    private readonly IPlayerRepository _playerRepository;

    public GameHubServiceTests()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new DatabaseContext(options);

        _gameRepository = new GameRepository(_context);
        _playerRepository = new PlayerRepository(_context);

        _playerService = new PlayerService(_playerRepository);
        _gameService = new GameService(_gameRepository);
        _gameHubService = new GameHubService(_playerRepository, _playerService, _gameRepository);
    }

    [Fact]
    public async Task JoinGame_WhenPlayerNotInGame_AddsPlayerToGame()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();

        // Arrange
        var playerId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        var connectionId = "abc123";

        var game = new Game(new List<Player>(), gameId);

        await _gameRepository.Add(GameMapper.ModelToEntity(game));

        // Act
        var result = await _gameHubService.JoinGame(playerId, gameId, connectionId);

        // Assert
        var gameEntity = await _gameRepository.GetById(gameId);
        Assert.Single(gameEntity.Players);
        Assert.Equal(playerId, gameEntity.Players[0].Id);
        Assert.Equal(connectionId, gameEntity.Players[0].ConnectionId);
    }

    [Fact]
    public async Task JoinGame_WhenPlayerAlreadyInGame_DoesNotDuplicatePlayer()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();

        // Arrange
        var playerId = Guid.NewGuid();
        var connectionId = "abc123";
        var gameId = Guid.NewGuid();

        var existingPlayer = new Player(playerId, "Test", Role.User, connectionId);
        var game = new Game(new List<Player>(), gameId);

        await _playerRepository.Add(PlayerMapper.ModelToEntity(existingPlayer));
        await _gameRepository.Add(GameMapper.ModelToEntity(game));

        // Act
        var result = await _gameHubService.JoinGame(playerId, gameId, connectionId);

        // Assert
        var gameEntity = await _gameRepository.GetById(gameId);
        Assert.Single(gameEntity.Players); // не добавился второй раз
        Assert.Equal(playerId, gameEntity.Players[0].Id);
    }
}
