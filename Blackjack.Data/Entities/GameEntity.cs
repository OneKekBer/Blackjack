using Blackjack.Data.Entities.Interfaces;
using Blackjack.GameLogic.Models;

namespace Blackjack.Data.Entities;

public class GameEntity : Entity
{
    public GameEntity()
    {
        
    }

    public GameEntity(List<PlayerEntity> players)
    {
        Players = players;
    }

    public List<PlayerEntity> Players { get; set; } = new List<PlayerEntity>();
    public GameStatus Status { get; set; } = GameStatus.WaitingForPlayers;
    public int Bet { get; set; } = 100;
    public int CurrentPlayerIndex { get; set; } = 0;
    public string Deck { get; set; }
}