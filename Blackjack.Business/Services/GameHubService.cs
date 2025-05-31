using Blackjack.Business.Mappers;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Context;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.Business.Services;

public class GameHubService : IGameHubService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerService _playerService;
    private readonly IGameRepository _gameRepository;

    public GameHubService(IPlayerRepository playerRepository, IPlayerService playerService,
        IGameRepository gameRepository)
    {
        _playerRepository = playerRepository;
        _playerService = playerService;
        _gameRepository = gameRepository;
    }

    public async Task JoinGame(Guid playerId, Guid gameId, string connectionId)
    {
        var validatedPlayer = await _playerService.GetValidatedPlayer(playerId, connectionId);

        var gameEntity = await _gameRepository.GetById(gameId);
        var isPlayerAlreadyInGame = gameEntity.Players
            .Exists(p => p.Id == playerId);
        
        if (isPlayerAlreadyInGame) return;
        
        gameEntity.Players.Add(PlayerMapper.ModelToEntity(validatedPlayer)); 
        await _gameRepository.Update(gameEntity);
    }
}