using Blackjack.GameLogic.Types;
using Blackjack.Presentation.Hubs;
using Xunit;

namespace Blackjack.UnitTests.Presentation;

public class GameHubDispatcherUnitTests
{
    [Fact]
    public async Task GetPlayerAction_WaitsUntilSet()
    {
        //Arrange
        var dispatcher = new GameHubDispatcher(null, null);
        var playerId = Guid.NewGuid();
        var expectedAction = PlayerAction.Hit;
        
        //Act
        var task = dispatcher.GetPlayerAction(Guid.NewGuid(), playerId);
        
        Assert.False(task.IsCompleted); //Doesnt count)

        dispatcher.SetPlayerAction(playerId, expectedAction);
        var actualAction = await task;
        
        //Assert
        Assert.True(task.IsCompleted);
        Assert.Equal(expectedAction, actualAction);
    }

    [Fact]
    public async Task MergePlayers_MergeCorrectly()
    {
        var dispatcher = new GameHubDispatcher(null, null);
    }
}
