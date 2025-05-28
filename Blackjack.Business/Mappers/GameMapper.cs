using Blackjack.Business.Helpers;
using Blackjack.Data.Entities;
using Blackjack.GameLogic.Models;

namespace Blackjack.Business.Mappers;

public static class GameMapper
{
    public static Game EntityToModel(GameEntity gameEntity)
    {
        var players = gameEntity.Players
            .Select(PlayerMapper.EntityToModel)
            .ToList();

        var deck = CardConverter.StringToCards(gameEntity.Deck).ToList();

        return new Game(
            players,
            deck,
            gameEntity.Id,
            gameEntity.Status,
            gameEntity.Bet
        )
        {
            CurrentPlayerIndex = gameEntity.CurrentPlayerIndex
        };
    }

    public static GameEntity ModelToEntity(Game game)
    {
        var playerEntities = game.Players
            .Select(PlayerMapper.ModelToEntity)
            .ToList();

        return new GameEntity(playerEntities)
        {
            Id = game.Id,
            Status = game.Status,
            Bet = game.Bet,
            CurrentPlayerIndex = game.CurrentPlayerIndex,
            Deck = CardConverter.CardToString(game.Deck)
        };
    }
}