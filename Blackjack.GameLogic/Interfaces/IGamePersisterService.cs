using Blackjack.GameLogic.Models;

namespace Blackjack.GameLogic.Interfaces;

public interface IGamePersisterService
{
    public Task SaveGame(Game game);
}