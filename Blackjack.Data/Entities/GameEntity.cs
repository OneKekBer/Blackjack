using Blackjack.Data.Entities.Interfaces;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.Data.Entities;

public class GameEntity : Entity
{
    public GameEntity()
    {
        
    }

    public GameEntity(
        Guid id,
        List<PlayerEntity> players,
        GameStatus status,
        int bet,
        int currentPlayerIndex,
        string deck)
    {
        Id = id;
        Players = players;
        Status = status;
        Bet = bet;
        CurrentPlayerIndex = currentPlayerIndex;
        Deck = deck;
    }

    public List<PlayerEntity> Players { get; set; } = new List<PlayerEntity>();
    public GameStatus Status { get; set; } = GameStatus.WaitingForPlayers;
    public int Bet { get; set; } = 100;
    public int CurrentPlayerIndex { get; set; } = 0;
    public string Deck { get; set; }
}