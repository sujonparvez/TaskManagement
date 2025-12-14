namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        System.Threading.Tasks.Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
