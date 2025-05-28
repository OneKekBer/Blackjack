using Blackjack.Business.Helpers;
using Blackjack.Business.Mappers;
using Blackjack.Data.Entities;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;
using Xunit;

namespace Blackjack.UnitTests.Business;

public class GameMapperUnitTests
{
    [Fact]
    public void EntityToModel_WhenConvertGameEntityToModel_ConvertCorrectly()
    {
        // Arrange
        var playerEntity = new PlayerEntity(
            Guid.NewGuid(),
            true,
            Role.User,
            "Alice",
            1500,
            "0-0 1-1"
        );

        var gameEntity = new GameEntity(new List<PlayerEntity> { playerEntity })
        {
            Bet = 200,
            CurrentPlayerIndex = 1,
            Status = GameStatus.Started,
            Deck = "0-0 1-1"
        };

        // Act
        var game = GameMapper.EntityToModel(gameEntity);

        // Assert
        Assert.Equal(gameEntity.Bet, game.Bet);
        Assert.Equal(gameEntity.CurrentPlayerIndex, game.CurrentPlayerIndex);
        Assert.Equal(gameEntity.Status, game.Status);
        Assert.Equal(gameEntity.Players.Count, game.Players.Count);
        Assert.Equal(CardConverter.StringToCards(gameEntity.Deck).Count, game.Deck.Count);
        Assert.Equal(CardConverter.StringToCards(gameEntity.Deck)[0].Rank, game.Deck[0].Rank);
        Assert.Equal(CardConverter.StringToCards(gameEntity.Deck)[0].Suits, game.Deck[0].Suits);
    }

    [Fact]
    public void ModelToEntity_WhenConvertGameModelToEntity_ConvertCorrectly()
    {
        // Arrange
        var player = new Player(
            isPlaying: true,
            role: Role.User,
            name: "Dealer",
            balance: 1000,
            id: Guid.NewGuid(),
            cards: new List<Card> { new Card(Suits.Diamond, Rank.Ten) }
        );

        var game = new Game(
            players: new List<Player> { player },
            deck: new List<Card>
            {
                new Card(Suits.Heart, Rank.Nine),
                new Card(Suits.Spade, Rank.Jack)
            },
            id: Guid.NewGuid(),
            status: GameStatus.WaitingForPlayers,
            bet: 150
        )
        {
            CurrentPlayerIndex = 0
        };

        // Act
        var gameEntity = GameMapper.ModelToEntity(game);

        // Assert
        Assert.Equal(game.Bet, gameEntity.Bet);
        Assert.Equal(game.Status, gameEntity.Status);
        Assert.Equal(game.CurrentPlayerIndex, gameEntity.CurrentPlayerIndex);
        Assert.Equal(game.Deck.Count, CardConverter.StringToCards(gameEntity.Deck).Count);
        Assert.Equal(game.Players.Count, gameEntity.Players.Count);
        Assert.Equal(game.Players[0].Id, gameEntity.Players[0].Id);
    }
}
