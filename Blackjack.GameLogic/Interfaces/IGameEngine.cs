using Blackjack.GameLogic.Models;

namespace Blackjack.GameLogic.Interfaces;

public interface IGameEngine
{
    public void InitGame(Game game);

    public void Start();
}