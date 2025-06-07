using Blackjack.Business.Mappers;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Interfaces;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.Business.Services;

public class GameHubService : IGameHubService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerService _playerService;
    private readonly IGameRepository _gameRepository;
    private readonly IGameHubDispatcher _gameHubDispatcher; 
    private readonly GameEngine _gameEngine;

    public GameHubService(IPlayerRepository playerRepository, IPlayerService playerService, IGameRepository gameRepository, IGameHubDispatcher gameHubDispatcher)
    {
        _playerRepository = playerRepository;
        _playerService = playerService;
        _gameRepository = gameRepository;
        _gameHubDispatcher = gameHubDispatcher;
        _gameEngine = new GameEngine(gameHubDispatcher, gameHubDispatcher);
    }

    public async Task<Game> JoinGame(Guid userId, Guid gameId, string connectionId)
    {
        var gameEntity = await _gameRepository.GetById(gameId);
        
        var isPlayerAlreadyInGame = gameEntity.Players
            .Exists(p => p.UserId == userId);
        
        if (!isPlayerAlreadyInGame)
        {
            var newPlayer = new Player(Guid.NewGuid(), "", Role.User, connectionId, userId);
            var newPlayerEntity = PlayerMapper.ModelToEntity(newPlayer);
            
            await _playerRepository.Add(newPlayerEntity);
            gameEntity.Players.Add(newPlayerEntity);
            await _gameRepository.Save();
        }
        
        return GameMapper.EntityToModel(gameEntity);
    }
    
    
    public async Task GetPlayerAction(Guid gameId, Guid playerId, PlayerAction action)
    {
        _gameHubDispatcher.SetPlayerAction(playerId, action);
    }
    
    public async Task<Game> StartGame(Guid gameId)
    {
        var game = GameMapper.EntityToModel(await _gameRepository.GetById(gameId));
        
        _gameEngine.InitGame(game);
        _gameEngine.Start();
        
        await _gameRepository.Save();
        
        return game;
    }
}