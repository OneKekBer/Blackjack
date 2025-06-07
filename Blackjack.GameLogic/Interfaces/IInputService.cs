using Blackjack.GameLogic.Types;

namespace Blackjack.GameLogic.Interfaces;

public interface IInputService
{
    public Task<PlayerAction> GetPlayerAction(Guid gameId, Guid playerId); 
}