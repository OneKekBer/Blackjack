using Blackjack.Business.Helpers;
using Blackjack.Data.Entities;
using Blackjack.GameLogic.Models;

namespace Blackjack.Business.Mappers;

public static class PlayerMapper
{
    public static Player EntityToModel(PlayerEntity entity)
    {
        var player = new Player(
            entity.Id,
            entity.Name,
            entity.Role,
            entity.ConnectionId
        )
        {
            IsPlaying = entity.IsPlaying,
            Balance = entity.Balance
        };

        player.Cards.AddRange(CardConverter.StringToCards(entity.Cards));
        return player;
    }

    public static PlayerEntity ModelToEntity(Player player)
    {
        return new PlayerEntity(
            player.Id,
            player.IsPlaying,
            player.Role,
            player.Name,
            player.Balance,
            CardConverter.CardToString(player.Cards),
            player.ConnectionId
        );
    }
}
