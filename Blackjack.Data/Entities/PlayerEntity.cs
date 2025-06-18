using Blackjack.Data.Entities.Interfaces;
using Blackjack.GameLogic.Types;

namespace Blackjack.Data.Entities;

public class PlayerEntity : Entity
{
    public PlayerEntity()
    {
        
    }
    
    public PlayerEntity(Guid id, bool isPlaying, Role role, string name, int balance, string cards, string connectionId, Guid? userId)
    {
        Id = id;
        IsPlaying = isPlaying;
        Role = role;
        Name = name;
        Balance = balance;
        Cards = cards;
        ConnectionId = connectionId;
        UserId = userId;
    }
    
    public bool IsPlaying { get; } = true; 
    public Role Role { get; }
    public string Name { get; set; }
    public int Balance { get; } = 1000;
    public string Cards { get; } = default!;
    public string ConnectionId { get; set; } = default!;
    public Guid? UserId { get; set; }
}