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
        var databaseContext = await _mockContext.DbContextFactory.CreateDbContextAsync();
        
        await databaseContext.Database.EnsureDeletedAsync();
        await databaseContext.Database.EnsureCreatedAsync();
        
        //Act
        await _mockContext.GameService.Create();
        
        //Assert
        Assert.Equal(1, databaseContext.Games.Count());
    }
    
    [Fact]
    public async Task GetAll_WhenCallMethod_GetsEmptyListOfGames()
    {
        var databaseContext = await _mockContext.DbContextFactory.CreateDbContextAsync();
        _mockContext.ClearCache();

        await databaseContext.Database.EnsureDeletedAsync();
        await databaseContext.Database.EnsureCreatedAsync();
        
        //Act
        var games = await _mockContext.GameService.GetAll();
        
        //Assert
        Assert.Empty(games);
    }
}