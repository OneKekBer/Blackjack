using Blackjack.GameLogic.Interfaces;
using Blackjack.GameLogic.Types;

namespace Blackjack.ConsoleApp.Services;

public class InputService : IInputService
{
    public Task<PlayerAction> GetPlayerAction(Guid gameId, Guid playerId)
    {
        string? playerAction = default;
        Console.WriteLine("0 - Hit, 1 - Stand");
        while (string.IsNullOrWhiteSpace(playerAction))
        {
            playerAction = Console.ReadLine();
        }
        
        var actionIndex = int.TryParse(playerAction, out var index) ? index : throw new FormatException(); // create better input

        return Task.FromResult((PlayerAction)actionIndex);
    }
}