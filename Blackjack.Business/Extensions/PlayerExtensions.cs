using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.Business.Extensions;

public static class PlayerExtensions
{
    public static IEnumerable<Guid> GetPlayersIds(this IEnumerable<Player> players)
    {
        return players
            .Where(p => p.Role == Role.User)
            .Select(p => p.Id);
    }
}