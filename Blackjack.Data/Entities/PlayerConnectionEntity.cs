using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blackjack.Data.Entities;

public class PlayerConnectionEntity
{
    [Key]
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public string ConnectionId { get; set; }
    public DateTime LastUpdated { get; set; }
    
    [ForeignKey("PlayerId")]
    public virtual PlayerEntity Player { get; set; }
}