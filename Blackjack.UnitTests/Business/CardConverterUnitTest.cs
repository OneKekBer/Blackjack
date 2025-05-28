using Blackjack.Business.Helpers;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;
using Xunit;
using Xunit.Abstractions;

namespace Blackjack.UnitTests.Business;

public class CardConverterUnitTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public CardConverterUnitTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void CardToString_WhenConvertCardToString_ConvertsCorrectly()
    {
        // Arrange
        var card = new Card(Suits.Club, Rank.Ace);
        
        // Act
        var cardString = CardConverter.CardToString(card);
        
        // Assert
        _testOutputHelper.WriteLine(cardString);
        Assert.Equal("0-0", cardString);
    }
    
    [Fact]
    public void CardsToString_WhenConvertListOfCardsToString_ConvertsCorrectly()
    {
        // Arrange
        var deck = new List<Card>()
        {
            new Card(Suits.Club, Rank.Ace),      
            new Card(Suits.Diamond, Rank.Ace),   
            new Card(Suits.Heart, Rank.Ace),     
        };
        
        // Act
        var cardString = CardConverter.CardToString(deck);
        
        // Assert
        _testOutputHelper.WriteLine(cardString);
        Assert.Equal("0-0 1-0 2-0", cardString);
    }
    
    [Fact]
    public void StringToCards_WhenGivenValidString_ReturnsCorrectCards()
    {
        // Arrange
        var input = "0-0 1-0 2-0";

        // Act
        var cards = CardConverter.StringToCards(input).ToList();

        // Assert
        Assert.Equal(3, cards.Count);

        Assert.Equal(Suits.Club, cards[0].Suits);
        Assert.Equal(Rank.Ace, cards[0].Rank);

        Assert.Equal(Suits.Diamond, cards[1].Suits);
        Assert.Equal(Rank.Ace, cards[1].Rank);

        Assert.Equal(Suits.Heart, cards[2].Suits);
        Assert.Equal(Rank.Ace, cards[2].Rank);
    }
}