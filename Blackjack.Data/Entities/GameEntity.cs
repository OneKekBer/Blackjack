using Blackjack.Data.Entities.Interfaces;
using Blackjack.GameLogic.Types;

namespace Blackjack.Data.Entities;

public class GameEntity : Entity
{
    public GameEntity()
    {
        
    }

    public GameEntity
    (
        Guid id,
        List<PlayerEntity> players,
        GameStatus status,
        int bet,
        string deck,
        List<Guid> turnQueue
        )
    {
        Id = id;
        Players = players;
        Status = status;
        Bet = bet;
        Deck = deck;
        TurnQueue = turnQueue;
    }

    public List<Guid> TurnQueue { get; set; } = new List<Guid>();
    public List<PlayerEntity> Players { get; } = new List<PlayerEntity>();
    public GameStatus Status { get; set; } = GameStatus.WaitingForPlayers;
    public int Bet { get; } = 100;
    public string Deck { get; set; } = "";
}