using Blackjack.ConsoleApp.View;
using Blackjack.GameLogic.Models;

namespace Blackjack.ConsoleApp.Mappers;

public static class PlayerMapper
{
    public static ViewPlayer ModelToView(Player model)
    {
        return new ViewPlayer(model.Id, model.Cards, model.Name, model.Balance);
    }

    public static IEnumerable<ViewPlayer> ModelToView(IEnumerable<Player> models)
    {
        return models.Select(ModelToView);
    }
}