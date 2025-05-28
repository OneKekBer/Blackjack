using Blackjack.Business.Helpers;
using Blackjack.Data.Entities;
using Blackjack.GameLogic.Models;

namespace Blackjack.Business.Mappers;

public static class PlayerMapper
{
    public static Player EntityToModel(PlayerEntity gameEntity)
    {
        return new Player(
            gameEntity.IsPlaying, 
            gameEntity.Role,
            gameEntity.Name,
            gameEntity.Balance,
            gameEntity.Id,
            CardConverter.StringToCards(gameEntity.Cards)
            );
    }

    public static PlayerEntity ModelToEntity(Player player)
    {
        return new PlayerEntity(
            player.Id,
            player.IsPlaying,
            player.Role,
            player.Name,
            player.Balance,
            CardConverter.CardToString(player.Cards)
        );
    }
}