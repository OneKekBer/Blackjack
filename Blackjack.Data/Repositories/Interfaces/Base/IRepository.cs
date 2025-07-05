namespace Blackjack.Data.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : class 
{
    public Task Add(TEntity entity, CancellationToken cancellationToken = default);
    public Task Delete(TEntity entity, CancellationToken cancellationToken = default);
    public Task<TEntity?> GetById(Guid id, CancellationToken cancellationToken = default);
    public Task Save(TEntity entity, CancellationToken cancellationToken = default);
}