using Blackjack.Business.Mappers;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;
using Blackjack.ServiceTests.Mock;
using Xunit;

namespace Blackjack.ServiceTests;

public class CachedGameRepositoryServiceTests : IClassFixture<MockContext>
{
    private readonly MockContext _mockContext;

    public CachedGameRepositoryServiceTests(MockContext mockContext)
    {
        _mockContext = mockContext;
    }

    
    
    public async Task Update_WhenTrySave_SavingCorrectly()
    {
        var databaseContext = await _mockContext.InitTest();

        //Arrange
        var playerId = Guid.NewGuid();
        var p1 = new Player(playerId, "A", Role.User, "huesos", null);
        var p2 = new Player(Guid.NewGuid(), "B", Role.User, "", null);
        var gameId = Guid.NewGuid();
        var game = new Game([p1, p2], gameId);
        
        //Act
        await _mockContext.GameRepository.Add(GameMapper.ModelToEntity(game));
        
        var gameFromDatabase = GameMapper.EntityToModel(await _mockContext.GameRepository.GetById(gameId));
        
        gameFromDatabase.Players.Single(p => p.Id == playerId).ConnectionId = "mrazz";
        
        await _mockContext.GameRepository.Save(GameMapper.ModelToEntity(gameFromDatabase));


    }
    
}
    
