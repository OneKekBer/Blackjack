using Blackjack.GameLogic.Interfaces;
using Blackjack.GameLogic.Types;

namespace Blackjack.Data.Interfaces;

public interface IGameHubDispatcher : IOutputService, IInputService
{
    void SetPlayerAction(Guid playerId, PlayerAction action);
}