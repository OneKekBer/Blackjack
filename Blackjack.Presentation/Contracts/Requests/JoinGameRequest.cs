namespace Blackjack.Presentation.Contracts.Requests;

public record JoinGameRequest(Guid UserId, Guid GameId);