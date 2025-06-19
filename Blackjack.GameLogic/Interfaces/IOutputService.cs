using Blackjack.GameLogic.Models;

namespace Blackjack.GameLogic.Interfaces;

public interface IOutputService
{ 
    public Task ShowPlayerHand(Guid gameId, Guid playerId, List<Card> cards, int score);
    public Task ShowResult(Guid gameId, string message, IEnumerable<Player> players);
    public Task ShowNewTurnPlayerId(Guid gameId, Guid currentPlayerId);
    public Task SendGameState(Game game);
}