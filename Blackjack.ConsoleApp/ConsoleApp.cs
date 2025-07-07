using Blackjack.ConsoleApp.Services;
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
            new OutputService(),
            new GamePersisterService()
            );
        var gameId = Guid.NewGuid();
        gameEngine.InitGame(new Game(new List<Player>() 
        {
            new Player(Guid.NewGuid(),"WOW", Role.User, Guid.NewGuid()),
            new Player(Guid.NewGuid(), "BOT NUM1", Role.Bot, null),

        }, gameId));
        gameEngine.Start(gameId);
    }
}