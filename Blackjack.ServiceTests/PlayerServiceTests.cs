using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Context;
using Blackjack.Data.Repositories.Interfaces;

namespace Blackjack.ServiceTests;

public class PlayerServiceTests
{
    private readonly IPlayerService _playerService;
    private readonly DatabaseContext _context;
    private readonly IPlayerRepository _playerRepository;

    /*public PlayerServiceTests()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new DatabaseContext(options);
        
        _playerRepository = new PlayerRepository(_context);
        _playerService = new PlayerService(_playerRepository);
    }*/

    /*[Fact]
    public async Task GetValidatedPlayer_ValidateNonExistentPlayer_ReturnValidPlayer()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
        
        //Arrange
        var newPlayerId = Guid.NewGuid();
        var connectionId = "wheheheh";
        
        //Act
        await _playerService.GetValidatedPlayer(newPlayerId, connectionId);
        
        //Assert
        var player = await _playerRepository.GetById(newPlayerId);
        Assert.NotNull(player);
        Assert.Equal(player.ConnectionId, connectionId);
    }
    
    [Fact]
    public async Task GetValidatedPlayer_ValidateExistentPlayer_ReturnValidPlayerAndOnePlayerInDatabase()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
        
        //Arrange
        var playerId = Guid.NewGuid();
        var connectionId = "wheheheh";
        
        var oldPlayer = new Player(playerId, "", Role.User, connectionId, Guid.NewGuid());
        
        //Act
        await _playerRepository.Add(PlayerMapper.ModelToEntity(oldPlayer));
        await _playerService.GetValidatedPlayer(playerId, connectionId);
        
        //Assert
        var player = await _playerRepository.GetById(playerId);
        Assert.NotNull(player);
        Assert.Equal(player.ConnectionId, connectionId);
        Assert.Equal(1, _context.Players.Count());
    }*/
}