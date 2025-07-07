using Blackjack.Data.Entities.Interfaces;
using Blackjack.GameLogic.Types;

namespace Blackjack.Data.Entities;

public class PlayerEntity : Entity
{
    public PlayerEntity()
    {
        
    }
    
    public PlayerEntity(Guid id, bool isPlaying, Role role, string name, int balance, string cards, Guid? userId)
    {
        Id = id;
        IsPlaying = isPlaying;
        Role = role;
        Name = name;
        Balance = balance;
        Cards = cards;
        UserId = userId;
    }
    
    public bool IsPlaying { get; set; } = true; 
    public Role Role { get; set; }
    public string Name { get; set; }
    public int Balance { get; set; } = 1000;
    public string Cards { get; set; } = default!;
    public Guid? UserId { get; set; }
}