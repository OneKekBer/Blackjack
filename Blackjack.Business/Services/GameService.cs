using Blackjack.Business.Mappers;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic.Models;

namespace Blackjack.Business.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;

    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }


    public async Task Create()
    {

    }
}