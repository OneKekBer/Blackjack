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
            Status = entity.Status,
            CurrentPlayerIndex = entity.CurrentPlayerIndex,
            Deck = CardConverter.StringToCards(entity.Deck).ToList()
        };

        return game;
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
            game.CurrentPlayerIndex,
            CardConverter.CardToString(game.Deck)
        );
    }
}
