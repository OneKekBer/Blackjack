using Blackjack.GameLogic.Models;

namespace Blackjack.GameLogic.Interfaces;

public interface IGameEngine
{
    public void InitGame(List<Player> players);

    public void Start();
}