using Blackjack.GameLogic.Handlers;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;
using Xunit;
using Xunit.Abstractions;

namespace Blackjack.UnitTests.GameLogic;

public class GameHandlerUnitTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public GameHandlerUnitTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void GetWinnersId_WhenOneWinner_ReturnsCorrectPlayerId()
    {
        // Arrange
        var winner = new Player(Guid.NewGuid(), "A", Role.User, "", null);
        var loser = new Player(Guid.NewGuid(), "B", Role.User, "", null);

        winner.Cards.Add(new Card(Suits.Club, Rank.Ten));
        winner.Cards.Add(new Card(Suits.Heart, Rank.Nine)); // Score: 19
        loser.Cards.Add(new Card(Suits.Club, Rank.Ten));
        loser.Cards.Add(new Card(Suits.Heart, Rank.King)); // Score: 20 (assume bust limit is 21, valid)

        var game = new Game(new List<Player> { winner, loser }, Guid.NewGuid());

        // Act
        var winners = GameHandler.GetWinnersId(game);

        // Assert
        Assert.Single(winners);
        Assert.Equal(loser.Id, winners.First());
    }

    [Fact]
    public void GetWinnersId_WhenMultipleWinners_ReturnsAllCorrectIds()
    {
        // Arrange
        var p1 = new Player(Guid.NewGuid(), "A", Role.User, "", null);
        var p2 = new Player(Guid.NewGuid(), "B", Role.User, "", null);
        p1.Cards.Add(new Card(Suits.Club, Rank.King)); 
        p2.Cards.Add(new Card(Suits.Heart, Rank.King));

        var game = new Game(new List<Player> { p1, p2 }, Guid.NewGuid());

        // Act
        var winners = GameHandler.GetWinnersId(game);

        // Assert
        Assert.Equal(2, winners.Count);
        Assert.Contains(p1.Id, winners);
        Assert.Contains(p2.Id, winners);
    }

    [Fact]
    public void ResetGame_ClearsPlayerCardsAndDeck()
    {
        // Arrange
        var player = new Player(Guid.NewGuid(), "A", Role.User, "", null);
        player.Cards.Add(new Card(Suits.Club, Rank.Ten));
        player.IsPlaying = false;

        var game = new Game(new List<Player> { player }, Guid.NewGuid());
        game.Deck.Clear(); // simulate used deck

        // Act
        GameHandler.ResetGame(game);

        // Assert
        Assert.Empty(player.Cards);
        Assert.True(player.IsPlaying);
        Assert.NotEmpty(game.Deck);
        Assert.Equal(0, game.CurrentPlayerIndex);
    }

    [Fact]
    public void GivePrizes_DistributesPotToWinners()
    {
        // Arrange
        var p1 = new Player(Guid.NewGuid(), "A", Role.User, "", null);
        var p2 = new Player(Guid.NewGuid(), "B", Role.User, "", null);
        var game = new Game(new List<Player> { p1, p2 }, Guid.NewGuid());

        var winners = new List<Guid> { p1.Id };

        // Act
        GameHandler.GivePrizes(game, winners);

        // Assert
        Assert.Equal(1100, p1.Balance); // won 100 from each
        Assert.Equal(900, p2.Balance);
    }
    
    [Fact]
    public void IsGameContinue_WhenTwoPlayersPlaying_ReturnsTrue()
    {
        // Arrange
        var players = new List<Player>
        {
            new Player(Guid.NewGuid(), "A", Role.User, "", null) { IsPlaying = true },
            new Player(Guid.NewGuid(), "B", Role.User, "", null) { IsPlaying = true }
        };

        // Act
        var result = GameHandler.IsGameContinue(players);

        // Assert
        Assert.True(result);
    }
}
