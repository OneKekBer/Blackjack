using Blackjack.Business.Mappers;
using Blackjack.Data.Entities;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;
using Xunit;
using Xunit.Abstractions;

namespace Blackjack.UnitTests.Business;

public class PlayerMapperUnitTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public PlayerMapperUnitTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void EntityToModel_WhenConvertEntityPlayerToModel_ConvertCorrectly()
    {
        // Arrange
        var cardString = "0-0 1-1";
        var entity = new PlayerEntity(
            Guid.NewGuid(),
            true,
            Role.User,
            "TestPlayer",
            1500,
            cardString
        );

        // Act
        var model = PlayerMapper.EntityToModel(entity);

        // Assert
        Assert.Equal(entity.Id, model.Id);
        Assert.Equal(entity.Name, model.Name);
        Assert.Equal(entity.Balance, model.Balance);
        Assert.Equal(entity.Role, model.Role);
        Assert.Equal(entity.IsPlaying, model.IsPlaying);

        var cards = model.Cards.ToList();
        Assert.Equal(2, cards.Count);
        Assert.Equal(Suits.Club, cards[0].Suits);
        Assert.Equal(Rank.Ace, cards[0].Rank);
        Assert.Equal(Suits.Diamond, cards[1].Suits);
        Assert.Equal(Rank.King, cards[1].Rank);
    }

    [Fact]
    public void ModelToEntity_WhenConvertPlayerModelToEntity_ConvertCorrectly()
    {
        // Arrange
        var player = new Player(
            isPlaying: true,
            role: Role.User,
            name: "John",
            balance: 2000,
            id: Guid.NewGuid(),
            cards: new List<Card>
            {
                new Card(Suits.Heart, Rank.Queen),
                new Card(Suits.Spade, Rank.Seven)
            }
        );

        // Act
        var entity = PlayerMapper.ModelToEntity(player);

        // Assert
        Assert.Equal(player.Id, entity.Id);
        Assert.Equal(player.Name, entity.Name);
        Assert.Equal(player.Balance, entity.Balance);
        Assert.Equal(player.Role, entity.Role);
        Assert.Equal(player.IsPlaying, entity.IsPlaying);
        
        
        _testOutputHelper.WriteLine(entity.Cards);
        Assert.Equal("2-2 3-7", entity.Cards);
    }
}
