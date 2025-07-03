using Blackjack.GameLogic.Extensions;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.Presentation.View;

public class PlayerView
{
    public PlayerView(Player player)
    {
        Id = player.Id;
        IsPlaying = player.IsPlaying;
        Role = player.Role;
        Name = player.Name;
        Balance = player.Balance;
        Score = player.Cards.GetScore();
        ConnectionId = player.ConnectionId;
        UserId = player.UserId;
        Cards = player.Cards;
    }

    public Guid Id { get; }
    public bool IsPlaying { get; } = true; 
    public Role Role { get; }
    public string Name { get; }
    public int Balance { get; } = 1000;
    public List<Card> Cards { get; } = new List<Card>();
    public int Score { get; } = 0;
    public string ConnectionId { get; }
    public Guid? UserId { get; }
}