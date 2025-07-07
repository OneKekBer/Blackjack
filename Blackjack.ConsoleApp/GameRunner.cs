using Blackjack.ConsoleApp.MemmoryStore;
using Blackjack.GameLogic;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.ConsoleApp;

public class GameRunner
{
    private readonly GameEngine _gameEngine;
    private readonly Store _store;
    
    public GameRunner(GameEngine gameEngine, Store store)
    {
        _gameEngine = gameEngine;
        _store = store;
    }

    public async Task StartGameAsync()
    {
        var gameId = Guid.NewGuid();
        var players = new List<Player>
        {
            new(Guid.NewGuid(), "Player 1", Role.User, Guid.NewGuid()),
            new(Guid.NewGuid(), "Dealer", Role.Bot, null)
        };
        var game = new Game(players, gameId);
        _gameEngine.InitGame(game);
        _store.RegisterGame(gameId, game);
        await _gameEngine.Start(gameId);
    }
}