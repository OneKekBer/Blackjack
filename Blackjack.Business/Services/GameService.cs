using Blackjack.Business.Mappers;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Other.Exceptions;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic.Models;

namespace Blackjack.Business.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IPlayerConnectionRepository _playerConnectionRepository;

    public GameService(IGameRepository gameRepository, IPlayerConnectionRepository playerConnectionRepository)
    {
        _gameRepository = gameRepository;
        _playerConnectionRepository = playerConnectionRepository;
    }
    
    public async Task Create(CancellationToken cancellationToken = default)
    {   
        var game = new Game(new List<Player>(), Guid.NewGuid());
        await _gameRepository.Add(GameMapper.ModelToEntity(game), cancellationToken);
    }

    public async Task DeleteAll(CancellationToken cancellationToken = default)
    {
        var games = await _gameRepository.GetAll(cancellationToken);
        foreach (var game in games)
        {
            var gameEntity = await _gameRepository.GetById(game.Id, cancellationToken);
            await _gameRepository.Delete(gameEntity!, cancellationToken);
        }
    }

    public async Task<IEnumerable<Game>> GetAll(CancellationToken cancellationToken = default)
    {
        var games = await _gameRepository.GetAll(cancellationToken);
        return GameMapper.EntityToModel(games);
    }
}