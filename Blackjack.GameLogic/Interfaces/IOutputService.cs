using Blackjack.GameLogic.Models;

namespace Blackjack.GameLogic.Interfaces;

public interface IOutputService
{ 
    public void ShowPlayerHand(Guid playerId, List<Card> cards, int score);
    
    public void ShowResult(string message, IEnumerable<Player> players);
    
}