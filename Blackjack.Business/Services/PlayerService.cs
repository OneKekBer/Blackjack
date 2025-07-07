using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Other.Exceptions;
using Blackjack.Data.Repositories.Interfaces;

namespace Blackjack.Business.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    
    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task ChangePlayerName(Guid playerId, Guid gameId, Guid userId, string newName, CancellationToken cancellationToken = default)
    {
        var player = await _playerRepository.GetById(playerId, cancellationToken);
        
        if(player is null || player.UserId != userId)
            throw new RenameProblemException($"Player with id {playerId} does not exist, or you dont have permission to rename the player.");
        
        player.Name = newName;
        await _playerRepository.Save(player, cancellationToken);
    }
    
    public Task AddBot(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}