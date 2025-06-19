namespace Blackjack.Business.Services.Interfaces;

public interface IPlayerService
{
    public Task ChangePlayerName(Guid playerId, Guid gameId, Guid userId, string newName);
}