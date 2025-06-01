using Blackjack.Business.Mappers;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic.Models;

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

    public async Task<Game> JoinGame(Guid playerId, Guid gameId, string connectionId)
    {
        var validatedPlayer = await _playerService.GetValidatedPlayer(playerId, connectionId);

        var gameEntity = await _gameRepository.GetById(gameId);
        var isPlayerAlreadyInGame = gameEntity.Players
            .Exists(p => p.Id == playerId);

        if (!isPlayerAlreadyInGame)
        {
            gameEntity.Players.Add(PlayerMapper.ModelToEntity(validatedPlayer));
        }
        
        return GameMapper.EntityToModel(gameEntity);
    }
}