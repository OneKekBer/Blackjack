using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blackjack.Data.Entities;

public class PlayerConnectionEntity
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
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}