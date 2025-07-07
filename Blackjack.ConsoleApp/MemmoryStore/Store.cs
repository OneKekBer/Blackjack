using Blackjack.GameLogic.Models;

namespace Blackjack.ConsoleApp.MemmoryStore;

public class Store
{
    private Dictionary<Guid, Game> _games { get; } = new Dictionary<Guid, Game>();

    public Game GetById(Guid id)
    {
        return _games[id];
    }

    public void Update(Game game)
    {
        _games[game.Id] = game;
    }
    
    public void RegisterGame(Guid id, Game game)
    {
        _games.Add(id, game);
    }
}