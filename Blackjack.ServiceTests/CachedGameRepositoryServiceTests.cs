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
    
    [Fact]
    public async Task Update_WhenTrySave_ConnectionIdUpdatedCorrectly()
    {
        var databaseContext = await _mockContext.InitTest();

        // Arrange
        var playerId = Guid.NewGuid();
        var p1 = new Player(playerId, "A", Role.User, "huesos", null);
        var p2 = new Player(Guid.NewGuid(), "B", Role.User, "", null);
        var gameId = Guid.NewGuid();
        var game = new Game([p1, p2], gameId);

        // Сохраняем игру в базу
        await _mockContext.GameRepository.Add(GameMapper.ModelToEntity(game));

        // Act
        // Загружаем из базы
        var gameFromDatabase = GameMapper.EntityToModel(await _mockContext.GameRepository.GetById(gameId));

        // Меняем ConnectionId
        var updatedConnectionId = "mrazz";
        gameFromDatabase.Players.Single(p => p.Id == playerId).ConnectionId = updatedConnectionId;
        
        Task.Run(async () =>
        {
            await Task.Delay(1000);
            await _mockContext.GameRepository.Save(GameMapper.ModelToEntity(gameFromDatabase));
        });
        
        // Сохраняем обратно
        await _mockContext.GameRepository.Save(GameMapper.ModelToEntity(gameFromDatabase));

        // Assert
        // Загружаем ещё раз из базы и проверяем
        var updatedGameFromDatabase = GameMapper.EntityToModel(await _mockContext.GameRepository.GetById(gameId));

        var updatedPlayer = updatedGameFromDatabase.Players.Single(p => p.Id == playerId);

        Assert.Equal(updatedConnectionId, updatedPlayer.ConnectionId);
    }
}
    
