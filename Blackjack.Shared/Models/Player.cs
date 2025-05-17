using Blackjack.Shared.Types;

namespace Blackjack.Shared.Models;


public class Player
{
    public Player(Role role, string name)
    {
        Role = role;
        Name = name;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Role Role { get; init; }
    
    public string Name { get; init; }

    public int Balance { get; set; } = 1000;
}