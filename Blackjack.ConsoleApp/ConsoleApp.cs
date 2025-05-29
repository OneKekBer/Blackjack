using Blackjack.ConsoleApp.Input;
using Blackjack.ConsoleApp.Output;
using Blackjack.GameLogic;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.ConsoleApp;

public class ConsoleApp
{
    public void Main()
    {
        var gameEngine = new GameEngine(
            new InputService(),
            new OutputService()
            );
        gameEngine.InitGame(new List<Player>() 
        {
            new Player(Guid.NewGuid(),"WOW",Role.User,""),
            new Player(Guid.NewGuid(), "BOT NUM1",Role.Bot, ""),

        });
        gameEngine.Start();
    }
}