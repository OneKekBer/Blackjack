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
        var player = await _playerRepository.GetById(playerId);
        if (player is null)
        {
            var newPLayer = new Player(playerId, "", Role.User, connectionId); // fix this
            await _playerRepository.Add(PlayerMapper.ModelToEntity(newPLayer));
            return newPLayer;
        }
        
        player.ConnectionId = connectionId;
        await _playerRepository.Update(player);
        return PlayerMapper.EntityToModel(player);
    }
}