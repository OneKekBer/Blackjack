using Blackjack.Data.Entities;
using Blackjack.Data.Other.Exceptions;
using Blackjack.Data.Repositories.Interfaces;
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

    /*private async Task<GameEntity> GetOrAddCache(Guid id, CancellationToken cancellationToken = default)
    {
        
    }*/
    
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
                    EvictionCallback = async (expiredKey, expiredValue, reason, state) => // will it run async trully ? mb Task.Run
                    {
                        _logger.LogDebug($"Eviction callback key:{expiredKey}. reason:{reason}");
                        var castedEntity = (GameEntity)expiredValue!;
                        
                        var databaseEntity = await _gameRepository.GetById(castedEntity.Id);
                        if (databaseEntity is null)
                        {
                            await _gameRepository.Add(castedEntity);
                            return;
                        }
                        
                        await _gameRepository.SaveGameEntity(castedEntity);
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

    public async Task<GameEntity?> GetById(Guid id, CancellationToken cancellationToken = default) // save tracking
    {
        var key = $"game:{id}";
        
        var cachedEntity = _memoryCache.Get(key);
        if (cachedEntity != null)
        {
            var castedEntity = (GameEntity)cachedEntity;
            _gameRepository.Attach(castedEntity);
            return castedEntity;
        }
        
        var databaseEntity = await _gameRepository.GetById(id, cancellationToken);
        if (databaseEntity == null) throw new NotFoundInDatabaseException("");
        
        AddCache(databaseEntity);
        return databaseEntity;
    }

    public async Task<GameEntity?> GetByIdAsNoTracking(Guid id, CancellationToken cancellationToken = default) // do not save tracking
    {
        var key = $"game:{id}";
        
        var cachedEntity = _memoryCache.Get(key);
        if (cachedEntity != null)
            return (GameEntity)cachedEntity;
        
        var databaseEntity = await _gameRepository.GetById(id, cancellationToken);
        if (databaseEntity == null) throw new NotFoundInDatabaseException("");

        AddCache(databaseEntity);
        return databaseEntity;
    }

    public Task<IEnumerable<GameEntity>> GetAll(CancellationToken cancellationToken = default)
    {
        return _gameRepository.GetAll(cancellationToken);
    }

    public async Task Update(GameEntity entity, CancellationToken cancellationToken = default) // need to save tracking 
    {
        
        await _gameRepository.Update(entity, cancellationToken);
    }

    public async Task Save(CancellationToken cancellationToken = default)
    {
        await _gameRepository.Save(cancellationToken);
    }

    public async Task Delete(GameEntity entity, CancellationToken cancellationToken = default)
    {
        await _gameRepository.Delete(entity, cancellationToken);
    }

    public async Task SaveGameEntity(GameEntity entity, CancellationToken cancellationToken = default)
    {
        var key = $"game:{entity.Id}";
        var cachedGame = _memoryCache.Get(key);

        if (cachedGame == null)
        {
            AddCache(entity);
            return;
        }
            
        AddCache(entity);
        //await _gameRepository.SaveGameEntity(entity, cancellationToken);
    }

    public void Attach(GameEntity entity)
    {
        _gameRepository.Attach(entity);
    }
}