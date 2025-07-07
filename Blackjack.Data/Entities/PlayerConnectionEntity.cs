using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blackjack.Data.Entities.Interfaces;

namespace Blackjack.Data.Entities;

public class PlayerConnectionEntity : Entity
{
    public PlayerConnectionEntity()
    {
        
    }

    public PlayerConnectionEntity(Guid playerId, string connectionId)
    {
        PlayerId = playerId;
        ConnectionId = connectionId;
    }
    
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PlayerId { get; set; }
    public string ConnectionId { get; set; }
}