using Blackjack.GameLogic.Models;

namespace Blackjack.GameLogic.Interfaces;

public interface IOutputService
{ 
    public Task ShowPlayerHand(Guid gameId, Guid playerId, List<Card> cards, int score);
    public Task SendResult(Guid gameId, string message, IEnumerable<Player> players);
    public Task SendNewTurnPlayerId(Guid gameId, IEnumerable<Player> players, Guid currentPlayerId);
    public Task SendGameState(Game game);
}