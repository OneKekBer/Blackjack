using Blackjack.GameLogic.Models;

namespace Blackjack.GameLogic.Interfaces;

public interface IGamePersisterService
{
    public void SaveGame(Game game);
}