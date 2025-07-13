using System.Threading.Tasks;
using System.Linq;
using Blackjack.ServiceTests.Mock;
using Xunit;

namespace Blackjack.ServiceTests;

public class GameServiceTests : IClassFixture<MockContext>
{
    private readonly MockContext _mockContext;

    public GameServiceTests(MockContext contextMockContext)
    {
        _mockContext = contextMockContext;
    }

    [Fact]
    public async Task Create_WhenCallMethod_CreatesGame()
    {
        var databaseContext = await _mockContext.InitTest();
        
        //Act
        await _mockContext.GameService.Create();
        
        //Assert
        Assert.Equal(1, databaseContext.Games.Count());
    }
    
    [Fact]
    public async Task GetAll_WhenCallMethod_GetsEmptyListOfGames()
    {
        await _mockContext.InitTest();
        
        //Act
        var games = await _mockContext.GameService.GetAll();
        
        //Assert
        Assert.Empty(games);
    }
}