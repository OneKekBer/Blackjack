using Blackjack.Presentation.View;

namespace Blackjack.Presentation.Hubs.Interfaces;

public interface IGameHubClient
{
    public Task SendGameState(GameView game);
    public Task SendNewGame(GameView game);
    public Task SendError(string message);
}
