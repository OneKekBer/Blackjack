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
            new Player(true,Role.User, "WOW"),
            new Player(true,Role.Bot, "BOT NUM1"),
            new Player(true,Role.Bot, "BOT NUM2"),
            new Player(true,Role.Bot, "BOT NUM3"),
            new Player(true,Role.Bot, "BOT NUM4"),
            new Player(true, Role.Bot, "BOT NUM5"),
            new Player(true, Role.Bot, "BOT NUM6")
        });
        gameEngine.Start();
    }
}