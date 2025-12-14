using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        System.Threading.Tasks.Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
        System.Threading.Tasks.Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize, string sortBy, bool isAscending, CancellationToken cancellationToken);
    }
}
