using Blackjack.Business.Helpers;
using Blackjack.Data.Entities;
using Blackjack.GameLogic.Models;

namespace Blackjack.Business.Mappers;

public static class GameMapper
{
    public static Game EntityToModel(GameEntity entity)
    {
        var players = entity.Players.Select(PlayerMapper.EntityToModel).ToList();
        var game = new Game(players, entity.Id)
        {
            Bet = entity.Bet,
            TurnQueue = new Queue<Guid>(entity.TurnQueue),
            Status = entity.Status,
            Deck = CardConverter.StringToCards(entity.Deck).ToList()
        };

        return game;
    }

    public static void CopyModelPropsToEntity(GameEntity entity, Game game) // this is 100% bullshit
    {
        entity.TurnQueue = game.TurnQueue.ToList();
        entity.Status = game.Status;
        entity.Deck = CardConverter.CardToString(game.Deck);
        entity.Bet = game.Bet;
        
        foreach (var player in game.Players)
        {
            var searchingPlayer = entity.Players
                .Find(p => p.Id == player.Id) ?? throw new Exception("Player entity not found");
            
            searchingPlayer.Balance = player.Balance;
            searchingPlayer.Cards = CardConverter.CardToString(player.Cards);
        }
    }
    
    public static List<Game> EntityToModel(IEnumerable<GameEntity> entity)
    {
        return entity.Select(EntityToModel).ToList();
    }

    public static GameEntity ModelToEntity(Game game)
    {
        var players = game.Players.Select(PlayerMapper.ModelToEntity).ToList();
        return new GameEntity(
            game.Id,
            players,
            game.Status,
            game.Bet,
            CardConverter.CardToString(game.Deck),
            game.TurnQueue.ToList()
        );
    }
}
