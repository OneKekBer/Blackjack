using Blackjack.GameLogic.Types;

namespace Blackjack.GameLogic.Models;



public class Player
{
    public Player(Role role, string name)
    {
        Role = role;
        Name = name;
    }

    public Guid Id { get; } = Guid.NewGuid();
    
    public bool IsPlaying { get; set; } = true; 
    
    public Role Role { get; }
    
    public string Name { get; }

    public int Balance { get; set; } = 1000;
    
    public List<Card> Cards { get; set; } = new();
}