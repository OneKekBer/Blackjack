using Blackjack.Business.Mappers;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Entities;
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
        var game = new Game(new List<Player>(), Guid.NewGuid());
        await _gameRepository.Add(GameMapper.ModelToEntity(game));
    }

    public async Task<IEnumerable<Game>> GetAll()
    {
        var games = await _gameRepository.GetAll();
        return GameMapper.EntityToModel(games);
    }
}