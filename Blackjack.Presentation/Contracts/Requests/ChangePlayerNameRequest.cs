namespace Blackjack.Presentation.Contracts.Requests;

public record ChangePlayerNameRequest(Guid PlayerId, Guid GameId, Guid UserId, string NewName);