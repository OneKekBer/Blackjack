using Blackjack.GameLogic.Interfaces;
using Blackjack.GameLogic.Types;

namespace Blackjack.ConsoleApp.Input;

public class InputService : IInputService
{
    public PlayerAction GetPlayerAction(Guid playerId)
    {
        string? playerAction = default;
        Console.WriteLine("0 - Hit, 1 - Stand");
        while (string.IsNullOrWhiteSpace(playerAction))
        {
            playerAction = Console.ReadLine();
        }
        
        var actionIndex = int.TryParse(playerAction, out var index) ? index : throw new FormatException(); // create better input

        return (PlayerAction)actionIndex;
    }
}