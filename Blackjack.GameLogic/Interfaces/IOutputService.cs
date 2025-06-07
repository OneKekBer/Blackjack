using Blackjack.GameLogic.Models;

namespace Blackjack.GameLogic.Interfaces;

public interface IOutputService
{ 
    public void ShowPlayerHand(Guid gameId, Guid playerId, List<Card> cards, int score);
    
    public void ShowResult(Guid gameId, string message, IEnumerable<Player> players);
    
}