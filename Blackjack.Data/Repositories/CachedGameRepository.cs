using Blackjack.Data.Entities;
using Blackjack.Data.Other.Exceptions;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.GameLogic.Types;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Blackjack.Data.Repositories;

public class CachedGameRepository : IGameRepository
{
    private readonly IGameRepository _gameRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<CachedGameRepository> _logger;
    
    public CachedGameRepository(IGameRepository gameRepository, IMemoryCache memoryCache, ILogger<CachedGameRepository> logger)
    {
        _gameRepository = gameRepository;
        _memoryCache = memoryCache;
        _logger = logger;
    }
    
    private void AddCache(GameEntity entity)
    {
        var key = $"game:{entity.Id}";
        
        _memoryCache.Set(key, entity, new MemoryCacheEntryOptions()
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(1),
            Size = 1024,
            PostEvictionCallbacks =
            {
                new PostEvictionCallbackRegistration()
                {
                    EvictionCallback = (expiredKey, expiredValue, reason, state) => // will it run async trully ? mb Task.Run (now i think yes)
                    {
                        Task.Run(async () =>
                        {
                            _logger.LogDebug($"Eviction callback key:{expiredKey}. reason:{reason}");
                            var castedEntity = (GameEntity)expiredValue!;

                            var databaseEntity = await _gameRepository.GetById(castedEntity.Id);
                            if (databaseEntity is null)
                            {
                                await _gameRepository.Add(castedEntity);
                                return;
                            }

                            await _gameRepository.Save(castedEntity);
                        });
                    }
                }
            }
        });
    }
    
    public async Task Add(GameEntity entity, CancellationToken cancellationToken = default)
    {
        AddCache(entity);
        await _gameRepository.Add(entity, cancellationToken);
    }

    public async Task Save(GameEntity entity, CancellationToken cancellationToken = default)
    {
        Console.WriteLine(entity.Players.First(e => e.Role == Role.User).ConnectionId);
        AddCache(entity);
        await _gameRepository.Save(entity, cancellationToken);
    }

    public async Task<GameEntity?> GetById(Guid id, CancellationToken cancellationToken = default) // save tracking
    {
        var key = $"game:{id}";
        
        var cachedEntity = _memoryCache.Get(key);
        if (cachedEntity != null)
        {
            var castedEntity = (GameEntity)cachedEntity;
            await _gameRepository.Attach(castedEntity, cancellationToken);
            return castedEntity;
        }
        
        var databaseEntity = await _gameRepository.GetById(id, cancellationToken);
        if (databaseEntity == null) throw new NotFoundInDatabaseException("");
        
        AddCache(databaseEntity);
        return databaseEntity;
    }
    
    public Task<IEnumerable<GameEntity>> GetAll(CancellationToken cancellationToken = default)
    {
        return _gameRepository.GetAll(cancellationToken);
    }
    
    public async Task Delete(GameEntity entity, CancellationToken cancellationToken = default)
    {
        _memoryCache.Remove(entity.Id);
        await _gameRepository.Delete(entity, cancellationToken);
    }

    public async Task Attach(GameEntity entity, CancellationToken cancellationToken = default)
    {
        await _gameRepository.Attach(entity, cancellationToken);
    }
}