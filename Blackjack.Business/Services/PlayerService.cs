using Blackjack.Business.Mappers;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.Business.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }
    
    public async Task<Player> GetValidatedPlayer(Guid playerId, string connectionId)
    {
        var playerEntity = await _playerRepository.GetById(playerId);

        if (playerEntity is null)
        {
            var newPlayer = new Player(playerId, "", Role.User, connectionId);
            var newEntity = PlayerMapper.ModelToEntity(newPlayer);
            await _playerRepository.Add(newEntity);
            return newPlayer;
        }

        playerEntity.ConnectionId = connectionId;
        await _playerRepository.Save(); 
        return PlayerMapper.EntityToModel(playerEntity);
    }

}