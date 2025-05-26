using Blackjack.Data.Entities.Interfaces;

namespace Blackjack.Data.Entities;

public class GameEntity : Entity
{
    public GameEntity()
    {
        
    }

    public GameEntity(List<PlayerEntity> players)
    {
        Players = players;
    }

    public List<PlayerEntity> Players { get; set; } = new List<PlayerEntity>();
}