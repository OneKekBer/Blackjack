using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Entities;
using Blackjack.Data.Other.Exceptions;
using Blackjack.Data.Repositories.Interfaces;

namespace Blackjack.Business.Services;

public class PlayerConnectionService : IPlayerConnectionService
{
    private readonly IPlayerConnectionRepository _connectionRepository;

    public PlayerConnectionService(IPlayerConnectionRepository connectionRepository)
    {
        _connectionRepository = connectionRepository;
    }

    public async Task ChangePlayerConnectionId(Guid playerId, string newConnectionId, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionRepository.GetByPlayerId(playerId, cancellationToken) 
                         ?? throw new NotFoundInDatabaseException("");
        
        connection.ConnectionId = newConnectionId;
        connection.LastUpdated = DateTime.UtcNow;
        await _connectionRepository.Save(connection, cancellationToken);
    }

    public async Task<string> GetPlayerConnectionId(Guid playerId, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionRepository.GetByPlayerId(playerId, cancellationToken) 
            ?? throw new NotFoundInDatabaseException("");

        return connection.ConnectionId;
    }
    //is this overload okay? im not sure about overloads in interfaces, i can rename in just GetPlayerConnectionsIds
    public async Task<IEnumerable<string>> GetPlayerConnectionId(IEnumerable<Guid> playersIds, CancellationToken cancellationToken = default)
    {
        var strings = new List<string>();
        foreach (var id in playersIds)
        {
            strings.Add(await GetPlayerConnectionId(id, cancellationToken));
        }
        
        return strings;
    }

    public async Task AddNewPlayerConnection(Guid playerId, string connectionId, CancellationToken cancellationToken = default)
    {
        var connection = new PlayerConnectionEntity(playerId, connectionId);
        await _connectionRepository.Add(connection, cancellationToken);
    }
}