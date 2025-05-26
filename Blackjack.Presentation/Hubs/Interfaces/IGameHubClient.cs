namespace Blackjack.Presentation.Hubs.Interfaces;

public interface IGameHubClient
{
    public Task GameStarted();

    public Task SendPlayerAction();
}
