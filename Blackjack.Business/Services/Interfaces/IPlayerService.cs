using Blackjack.GameLogic.Models;

namespace Blackjack.Business.Services.Interfaces;

public interface IPlayerService
{
    public Task<Player> GetValidatedPlayer(Guid playerId, string connectionId);
}