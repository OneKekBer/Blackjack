using Blackjack.Shared.Interfaces;
using Blackjack.Shared.Models;

namespace Blackjack.ConsoleApp.Output;

public class OutputService : IOutputService
{
    public void ShowPlayerHand(List<Card> cards, int score)
    {
        Console.WriteLine("Your Hand:");
        foreach (var card in cards)
        {
            Console.WriteLine($"- {card.Rank} of {card.Suits}");
        }
        
        Console.WriteLine($"\nTotal Score: {score}");
    }

    public void ShowBotHand(List<Card> cards, int score)
    {
        throw new NotImplementedException();
    }
}
