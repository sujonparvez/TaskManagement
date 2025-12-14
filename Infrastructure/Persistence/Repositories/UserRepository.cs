using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository(AppDbContext _context) : IUserRepository
    {
        public async System.Threading.Tasks.Task<Domain.Entities.User> AddAsync(Domain.Entities.User entity, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(entity, cancellationToken);
            return entity;
        }
        public async System.Threading.Tasks.Task UpdateAsync(Domain.Entities.User entity, CancellationToken cancellationToken)
        {
            _context.Users.Update(entity);
            await System.Threading.Tasks.Task.CompletedTask;
        }
        public async System.Threading.Tasks.Task DeleteAsync(Domain.Entities.User entity, CancellationToken cancellationToken)
        {
            _context.Users.Remove(entity);
            await System.Threading.Tasks.Task.CompletedTask;
        }

        public async System.Threading.Tasks.Task<Domain.Entities.User?> GetByUserNameAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);
        }

        public async System.Threading.Tasks.Task<Domain.Entities.User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public IQueryable<Domain.Entities.User> Query()
        {
            return _context.Users;  
        }
        public async System.Threading.Tasks.Task<IEnumerable<Domain.Entities.User>> GetAllAsync(int pageNumber, int pageSize, string sortBy, bool isAscending, CancellationToken cancellationToken)
        {
            var query = _context.Users.AsQueryable();

            // Simple sorting implementation (expand as needed)
            query = sortBy.ToLower() switch
            {
                "fullname" => isAscending ? query.OrderBy(u => u.FullName) : query.OrderByDescending(u => u.FullName),
                "email" => isAscending ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email),
                _ => query.OrderBy(u => u.Id)
            };

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }


    }

}
