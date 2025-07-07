using Blackjack.ConsoleApp.MemmoryStore;
using Blackjack.GameLogic.Interfaces;
using Blackjack.GameLogic.Models;

namespace Blackjack.ConsoleApp.Services;

public class GamePersisterService : IGamePersisterService
{
    private readonly Store _store;

    public GamePersisterService(Store store)
    {
        _store = store;
    }

    public Task SaveGame(Game game)
    {
        _store.Update(game);
        return Task.CompletedTask;
    }

    public Task<Game> LoadGame(Guid gameId)
    {
        return Task.FromResult(_store.GetById(gameId));
    }

    public Task SaveGameAndSendState(Game game)
    {
        _store.Update(game);
        return Task.CompletedTask;
    }
}