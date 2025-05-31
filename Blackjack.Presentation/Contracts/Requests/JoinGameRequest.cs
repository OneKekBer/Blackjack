namespace Blackjack.Presentation.Contracts.Requests;

public record JoinGameRequest(Guid PlayerId, Guid GameId);