using Blackjack.GameLogic.Types;

namespace Blackjack.Presentation.Contracts.Requests;

public record GetPlayerActionRequest(Guid GameId, Guid PlayerId, PlayerAction Action);