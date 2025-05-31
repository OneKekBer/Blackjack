using Blackjack.Business.Services;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Context;
using Blackjack.Data.Repositories;
using Blackjack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blackjack.ServiceTests;

public class GameServiceTests
{
    private readonly IGameService _gameService;
    private readonly DatabaseContext _context;
    private readonly IGameRepository _gameRepository;

    public GameServiceTests()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new DatabaseContext(options);
        
        _gameRepository = new GameRepository(_context);
        _gameService = new GameService(_gameRepository);
    }

    [Fact]
    public async Task Create_WhenCallMethod_CreatesGame()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
        
        //Act
        await _gameService.Create();
        
        //Assert
        Assert.Equal(1, _context.Games.Count());
    }
    
    [Fact]
    public async Task GetAll_WhenCallMethod_GetsEmptyListOfGames()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
        
        //Act
        var games = await _gameService.GetAll();
        
        //Assert
        Assert.Empty(games);
    }
}