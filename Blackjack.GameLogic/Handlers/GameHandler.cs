using Blackjack.GameLogic.Extensions;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Other;

namespace Blackjack.GameLogic.Handlers;

public static class GameHandler
{
    public static List<Guid> GetWinnersId(Game game)
    {
        var maxScore = 0;
        var maxScorePlayerIds = new List<Guid>();
            
        foreach (var player in game.Players)
        {
            var playerScore = player.Cards.GetScore();
            if (playerScore > Constants.Limit) continue;
            
            if (playerScore > maxScore)
            {
                maxScore = player.Cards.GetScore();
                maxScorePlayerIds.Clear();
                maxScorePlayerIds.Add(player.Id);
            }
            else if (playerScore == maxScore)
                maxScorePlayerIds.Add(player.Id);
        }
        
        return maxScorePlayerIds;
    }
    
    public static void ResetGame(Game game)
    {
        game.Players.ForEach(player =>
        {
            player.Cards.Clear();
            player.IsPlaying = true;
        });
        game.CurrentPlayerIndex = 0;
        game.Deck = DeckHandler.NewDeck();
    }
    
    public static void GivePrizes(Game game, List<Guid> winnersIds)
    {
        if (winnersIds.Count ==  0) return;
        var pot = 0;

        foreach (var player in game.Players)
        {
            player.Balance -= game.Bet;
            pot += game.Bet;
        }

        foreach (var winnerId in winnersIds)
        {
            game.Players.Find(p => p.Id == winnerId).Balance += (pot / winnersIds.Count);
        }
    }
    
    public static Card GetCard(Game game)
    {
        var random = new Random();
        var randomIndex = random.Next(game.Deck.Count - 1);
        var card = game.Deck[randomIndex];
        game.Deck.Remove(card);
        
        return card;    
    }

    public static void CheckPlayers(Game game)
    {
        foreach (var player in game.Players.ToList())
        {
            if (player.Balance < game.Bet)
            {
                game.Players.Remove(player);
            }
        }
    }
    
    public static bool IsGameContinue(List<Player> players)
    {
        var activeUsers = 0;
        
        foreach (var player in players)
        {
            if (player.IsPlaying) activeUsers += 1;
            if (activeUsers >= 2) return true;
        }
        
        return false;
    }
}