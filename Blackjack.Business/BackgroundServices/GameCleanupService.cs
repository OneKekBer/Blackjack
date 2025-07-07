using Blackjack.Data.Other.Handlers.Base;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blackjack.Business.BackgroundServices;

public class GameCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<GameCleanupService> _logger;
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromHours(6);
    private readonly TimeSpan _maxGameAge = TimeSpan.FromDays(1);

    public GameCleanupService(IServiceScopeFactory scopeFactory, ILogger<GameCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Game cleanup service started");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupOldGamesAsync(stoppingToken);
                await Task.Delay(_cleanupInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during game cleanup");
            }
        }
        
        _logger.LogInformation("Game cleanup service stopped");
    }

    private async Task CleanupOldGamesAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var gameRepository = scope.ServiceProvider.GetService<IGameRepository>();
        var transactionHandler = scope.ServiceProvider.GetService<ITransactionHandler>();
        var playerConnectionRepository = scope.ServiceProvider.GetService<IPlayerConnectionRepository>();
        var playerRepository = scope.ServiceProvider.GetService<IPlayerRepository>();
        
        var cutoffDate = DateTime.UtcNow - _maxGameAge;
        var oldGames = (await gameRepository.GetAll(cancellationToken))
            .Where(g => g.UpdatedAt == default || g.UpdatedAt < cutoffDate)
            .ToList();
        
        if (!oldGames.Any())
        {
            _logger.LogDebug("No old games found for cleanup");
            return;
        }

        _logger.LogInformation("Found {Count} games older than {Days} days for cleanup", 
            oldGames.Count, _maxGameAge.TotalDays);

        foreach (var game in oldGames)
        {
            var transaction = await transactionHandler.StartTransaction(cancellationToken);

            try
            {
                foreach (var player in game.Players)
                {
                    if (player.Role == Role.User)
                    {
                        var playerConnection = await playerConnectionRepository.GetByPlayerId(player.Id, cancellationToken);
                        await playerConnectionRepository.Delete(playerConnection, cancellationToken);
                    }
                    
                    await playerRepository.Delete(player, cancellationToken);
                }
                await gameRepository.Delete(game, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                
                _logger.LogDebug("Deleted game {GameId} (last updated: {Updated})", 
                    game.Id, game.UpdatedAt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete game {GameId}", game.Id);
                await transaction.RollbackAsync(cancellationToken);
            }
        }
    }
}