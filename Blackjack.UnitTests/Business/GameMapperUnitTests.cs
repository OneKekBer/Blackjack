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
        var playerId = Guid.NewGuid();
        var playerEntity = new PlayerEntity(
            playerId,
            true,
            Role.User,
            "Alice",
            1500,
            "0-0 1-1",
            "conn123",
            Guid.NewGuid()
        );

        var turnQueue = new List<Guid>();

        var gameEntity = new GameEntity(
            Guid.NewGuid(),
            new List<PlayerEntity> { playerEntity },
            GameStatus.Started,
            200,
            "0-0 1-1",
            turnQueue
        );

        // Act
        var game = GameMapper.EntityToModel(gameEntity);

        // Assert
        Assert.Equal(gameEntity.Bet, game.Bet);
        Assert.Equal(gameEntity.Status, game.Status);
        Assert.Equal(gameEntity.Players.Count, game.Players.Count);

        Assert.Equal(CardConverter.StringToCards(gameEntity.Deck).Count, game.Deck.Count);
        Assert.Equal(CardConverter.StringToCards(gameEntity.Deck)[0].Rank, game.Deck[0].Rank);
        Assert.Equal(CardConverter.StringToCards(gameEntity.Deck)[0].Suits, game.Deck[0].Suits);
        
        Assert.Equal(gameEntity.TurnQueue.Count, game.TurnQueue.Count);
        //Assert.Equal(gameEntity.TurnQueue[0], game.TurnQueue.Peek());
    }

    [Fact]
    public void EntityToModel_WhenConvertGameEntityToModelPlayerIsEmpty_ConvertCorrectly()
    {
        // Arrange
        var gameEntity = new GameEntity(
            Guid.NewGuid(),
            new List<PlayerEntity>(),
            GameStatus.Started,
            200,
            "0-0 1-1",
            new List<Guid>()
        );

        // Act
        var game = GameMapper.EntityToModel(gameEntity);

        // Assert
        Assert.Equal(gameEntity.Bet, game.Bet);
        Assert.Equal(gameEntity.Status, game.Status);
        Assert.Equal(gameEntity.Players.Count, game.Players.Count);

        Assert.Equal(CardConverter.StringToCards(gameEntity.Deck).Count, game.Deck.Count);
        Assert.Equal(CardConverter.StringToCards(gameEntity.Deck)[0].Rank, game.Deck[0].Rank);
        Assert.Equal(CardConverter.StringToCards(gameEntity.Deck)[0].Suits, game.Deck[0].Suits);

        Assert.Empty(game.TurnQueue);
    }

    [Fact]
    public void ModelToEntity_WhenConvertGameModelToEntity_ConvertCorrectly()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var player = new Player(
            id: playerId,
            name: "Dealer",
            role: Role.User,
            connectionId: "conn456",
            Guid.NewGuid()
        )
        {
            IsPlaying = true,
            Balance = 1000
        };
        player.Cards.Add(new Card(Suits.Diamond, Rank.Ten));

        var game = new Game(
            players: new List<Player> { player },
            id: Guid.NewGuid()
        )
        {
            Status = GameStatus.WaitingForPlayers,
            Bet = 150,
            Deck = new List<Card>
            {
                new Card(Suits.Heart, Rank.Nine),
                new Card(Suits.Spade, Rank.Jack)
            }
        };

        game.TurnQueue.Enqueue(playerId);

        // Act
        var gameEntity = GameMapper.ModelToEntity(game);

        // Assert
        Assert.Equal(game.Bet, gameEntity.Bet);
        Assert.Equal(game.Status, gameEntity.Status);

        Assert.Equal(game.Deck.Count, CardConverter.StringToCards(gameEntity.Deck).Count);
        Assert.Equal(game.Players.Count, gameEntity.Players.Count);
        Assert.Equal(game.Players[0].Id, gameEntity.Players[0].Id);
        
        Assert.Equal(game.TurnQueue.Count, gameEntity.TurnQueue.Count);
        Assert.Equal(game.TurnQueue.Peek(), gameEntity.TurnQueue[0]);
    }
}
