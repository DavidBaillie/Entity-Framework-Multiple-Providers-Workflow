namespace EntityFrameworkMultipleProvidersWorkflow.Api.Repositories
{
    public interface IAsyncRepository<T>
    {
        Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
