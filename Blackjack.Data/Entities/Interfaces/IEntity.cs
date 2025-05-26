namespace Blackjack.Data.Entities.Interfaces;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}