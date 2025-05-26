using Blackjack.GameLogic.Types;

namespace Blackjack.GameLogic.Interfaces;

public interface IInputService
{
    public PlayerAction GetPlayerAction(Guid playerId); 
}