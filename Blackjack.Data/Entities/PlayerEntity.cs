using Blackjack.Data.Entities.Interfaces;

namespace Blackjack.Data.Entities;

public class PlayerEntity : Entity
{
    public PlayerEntity()
    {
        
    }

    public PlayerEntity(string name)
    {
        Name = name;
    }
    
    public string Name { get; set; } = string.Empty;
}