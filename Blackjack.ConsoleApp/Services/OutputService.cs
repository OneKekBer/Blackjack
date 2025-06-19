using Blackjack.GameLogic.Extensions;
using Blackjack.GameLogic.Interfaces;
using Blackjack.GameLogic.Models;

namespace Blackjack.ConsoleApp.Services;

public class OutputService : IOutputService
{
    public Task ShowPlayerHand(Guid gameId,Guid playerId, List<Card> cards, int score)
    {
        Console.WriteLine("Your Hand:");
        foreach (var card in cards)
        {
            Console.WriteLine($"- {card.Rank} of {card.Suits}");
        }
        
        Console.WriteLine($"\nTotal Score: {score}");
        return Task.CompletedTask;
    }

    public Task ShowResult(Guid gameId, string message, IEnumerable<Player> players)
    {
        Console.WriteLine(message);

        foreach (var player in players)
        {
            var cards = string.Join(", ", player.Cards.Select(card => $"{card.Rank} of {card.Suits}"));
            Console.WriteLine($"Player: {player.Name} | Cards: {cards} | Score: {player.Cards.GetScore()} | Balance: {player.Balance}");
        }

        Console.WriteLine();
        return Task.CompletedTask;
    }

    public Task ShowNewTurnPlayerId(Guid gameId, Guid currentPlayerId)
    {
        return Task.CompletedTask;
    }

    public Task SendGameState(Game game)
    {
        return Task.CompletedTask;
    }
}
