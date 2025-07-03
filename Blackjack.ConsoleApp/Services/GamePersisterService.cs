using Blackjack.GameLogic.Interfaces;
using Blackjack.GameLogic.Models;

namespace Blackjack.ConsoleApp.Services;

public class GamePersisterService : IGamePersisterService
{
    public async Task SaveGame(Game game)
    {
        return;
    }

    public Task<Game> LoadGame(Guid gameId)
    {
        throw new NotImplementedException();
    }

    public async Task SaveGameAndSendState(Game game)
    {
        return;
    }
}