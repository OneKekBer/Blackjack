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
            cardString,
            Guid.NewGuid()

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
        Assert.Equal(Suits.Club, cards[0].Suits);    // 0 => Club
        Assert.Equal(Rank.Ace, cards[0].Rank);       // 0 => Ace
        Assert.Equal(Suits.Diamond, cards[1].Suits); // 1 => Diamond
        Assert.Equal(Rank.King, cards[1].Rank);      // 1 => King
    }

    [Fact]
    public void ModelToEntity_WhenConvertPlayerModelToEntity_ConvertCorrectly()
    {
        // Arrange
        var player = new Player(
            id: Guid.NewGuid(),
            name: "John",
            role: Role.User,
            Guid.NewGuid()

        )
        {
            IsPlaying = true,
            Balance = 2000
        };
        player.Cards.AddRange(new List<Card>
        {
            new Card(Suits.Heart, Rank.Queen),
            new Card(Suits.Spade, Rank.Seven)
        });

        // Act
        var entity = PlayerMapper.ModelToEntity(player);

        // Assert
        Assert.Equal(player.Id, entity.Id);
        Assert.Equal(player.Name, entity.Name);
        Assert.Equal(player.Balance, entity.Balance);
        Assert.Equal(player.Role, entity.Role);
        Assert.Equal(player.IsPlaying, entity.IsPlaying);

        _testOutputHelper.WriteLine(entity.Cards);
        Assert.Equal("2-2 3-7", entity.Cards); // 2-2 = Heart-Queen, 3-7 = Spade-Seven
    }
}
