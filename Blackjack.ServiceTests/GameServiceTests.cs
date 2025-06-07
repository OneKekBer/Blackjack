using Blackjack.Business.Services;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Context;
using Blackjack.Data.Repositories;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.ServiceTests.Mock;
using Microsoft.EntityFrameworkCore;
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
        await _mockContext.DatabaseContext.Database.EnsureDeletedAsync();
        await _mockContext.DatabaseContext.Database.EnsureCreatedAsync();
        
        //Act
        await _mockContext.GameService.Create();
        
        //Assert
        Assert.Equal(1, _mockContext.DatabaseContext.Games.Count());
    }
    
    [Fact]
    public async Task GetAll_WhenCallMethod_GetsEmptyListOfGames()
    {
        await _mockContext.DatabaseContext.Database.EnsureDeletedAsync();
        await _mockContext.DatabaseContext.Database.EnsureCreatedAsync();
        
        //Act
        var games = await _mockContext.GameService.GetAll();
        
        //Assert
        Assert.Empty(games);
    }
}