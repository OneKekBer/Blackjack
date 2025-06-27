namespace Blackjack.Presentation.Contracts.Requests;

public record AddBotToLobbyRequest(Guid GameId, Guid PlayerId);