using Blackjack.GameLogic.Models;

namespace Blackjack.Presentation.Hubs.Interfaces;

public interface IGameHubClient
{
    public Task GameStarted();
    public Task SendPlayerAction();
    public Task SendGameState(Game game);
    public Task SendNewGame(Game game);
    public Task SendError(string message);
}
