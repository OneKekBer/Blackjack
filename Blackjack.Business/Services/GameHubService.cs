using Blackjack.Business.Mappers;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Interfaces;
using Blackjack.Data.Other.Exceptions;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.Business.Services;

public class GameHubService : IGameHubService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerConnectionService _playerConnectionService;
    private readonly IGameRepository _gameRepository;
    private readonly IGameHubDispatcher _gameHubDispatcher; 
    private readonly GameEngine _gameEngine;
    private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1); //rework, but how,
                                                                           //i even dont know what is SemaphoreSlim(( its just works
    public GameHubService
    (
        IPlayerRepository playerRepository,
        IGameRepository gameRepository,
        IGameHubDispatcher gameHubDispatcher, IPlayerConnectionService playerConnectionService)
    {
        _playerRepository = playerRepository;
        _gameRepository = gameRepository;
        _gameHubDispatcher = gameHubDispatcher;
        _playerConnectionService = playerConnectionService;
        _gameEngine = new GameEngine(gameHubDispatcher, gameHubDispatcher, gameHubDispatcher);
    }
    
    public async Task<Game?> JoinGame(Guid userId, Guid gameId, string connectionId, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            var gameEntity = await _gameRepository.GetById(gameId, cancellationToken);
            if (gameEntity is null)
                return null;

            var existingPlayer = gameEntity.Players
                .Find(p => p.UserId == userId);
            
            if (existingPlayer is null)
            {
                var playerId = Guid.NewGuid();
                var newPlayer = new Player(playerId, $"Player: {playerId.ToString().Substring(0, 4)}", Role.User, userId);
                var newPlayerEntity = PlayerMapper.ModelToEntity(newPlayer);
                await _playerConnectionService.AddNewPlayerConnection(playerId, connectionId, cancellationToken);
                await _playerRepository.Add(newPlayerEntity, cancellationToken);
                gameEntity.Players.Add(newPlayerEntity);
            }
            else
            {
                await _playerConnectionService.ChangePlayerConnectionId(existingPlayer.Id, connectionId, cancellationToken);   
            }
            await _gameRepository.Save(gameEntity, cancellationToken);
            
            return GameMapper.EntityToModel(gameEntity);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<bool> IsPlayerExists(Guid userId, Guid gameId, CancellationToken cancellationToken = default)
    {
        var gameEntity = await _gameRepository.GetById(gameId, cancellationToken);
        if (gameEntity is null)
            return false;

        return gameEntity.Players
            .Any(p => p.UserId == userId);
    }

    public async Task GetPlayerAction(Guid gameId, Guid playerId, PlayerAction action, CancellationToken cancellationToken = default)
    {
        var gameEntity = await _gameRepository.GetById(gameId, cancellationToken) 
                   ?? throw new NotFoundInDatabaseException($"In getting player action in game with id: {gameId} has not been found");
        
        Console.WriteLine($"TurnQueue GetPlayerAction: {string.Join(", ", gameEntity.TurnQueue)}");
        
        if (gameEntity.TurnQueue.First() != playerId)
        {
            return;
        }
        
        _gameHubDispatcher.SetPlayerAction(playerId, action);
    }

    public async Task<Game?> AddBotToLobby(Guid gameId, Guid userId, CancellationToken cancellationToken = default) 
        // total bullshit, i think i lost sense, purpose of entities, for what i add them? but maybe i wrong 
    {
        var gameEntity = await _gameRepository.GetById(gameId, cancellationToken) 
                         ?? throw new NotFoundInDatabaseException($"In starting game with id: {gameId} has not been found");
        
        if (gameEntity.Status == GameStatus.Started || gameEntity.Players.Count > 8)
            return null;

        var botId = Guid.NewGuid();
        var bot = new Player(botId, $"Bot:{botId.ToString().Substring(0, 4)}", Role.Bot, null);
        var botEntity = PlayerMapper.ModelToEntity(bot);

        await _playerRepository.Add(botEntity, cancellationToken);
        gameEntity.Players.Add(botEntity);
            
        await _gameRepository.Save(gameEntity, cancellationToken);
        return GameMapper.EntityToModel(gameEntity);
    }
    
    public async Task<Game> StartGame(Guid gameId, CancellationToken cancellationToken = default)
    {
        var gameEntity = await _gameRepository.GetById(gameId, cancellationToken)  // take entity from db
                         ?? throw new NotFoundInDatabaseException($"In starting game with id: {gameId} has not been found");
        
        var game = GameMapper.EntityToModel(gameEntity); // map entity -> model
        _gameEngine.InitGame(game); // edit model
        
        await _gameRepository.Save(GameMapper.ModelToEntity(game), cancellationToken);
        
        _gameEngine.Start(game.Id);
        
        return game;
    }
}