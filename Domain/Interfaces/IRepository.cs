namespace Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();
        System.Threading.Tasks.Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
        System.Threading.Tasks.Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        System.Threading.Tasks.Task UpdateAsync(T entity, CancellationToken cancellationToken);
        System.Threading.Tasks.Task DeleteAsync(T entity, CancellationToken cancellationToken);
    }
}
