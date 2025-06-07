using Blackjack.GameLogic.Types;

namespace Blackjack.GameLogic.Models;

public class Player
{
    public Player(Guid id, string name, Role role, string connectionId, Guid? userId)
    {
        Id = id;
        Role = role;
        Name = name;
        ConnectionId = connectionId;
        UserId = userId;
    }

    public Guid Id { get; }
    public bool IsPlaying { get; set; } = true; 
    public Role Role { get; }
    public string Name { get; }
    public int Balance { get; set; } = 1000;
    public List<Card> Cards { get; init; } = new List<Card>();
    public string ConnectionId { get; set; }
    public Guid? UserId { get; set; }
}