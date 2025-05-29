using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Repositories.Interfaces;

namespace Blackjack.Business.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }
    
    
}