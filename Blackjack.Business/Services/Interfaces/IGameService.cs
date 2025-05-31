using Blackjack.GameLogic.Models;

namespace Blackjack.Business.Services.Interfaces;

public interface IGameService
{
    public Task Create();
    
    Task<IEnumerable<Game>> GetAll();
}