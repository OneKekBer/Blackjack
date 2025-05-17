using Blackjack.Shared.Models;

namespace Blackjack.Shared.Interfaces;

public interface IOutputService
{ 
    public void ShowPlayerHand(List<Card> cards, int score);
    
    public void ShowBotHand(List<Card> cards, int score);
}