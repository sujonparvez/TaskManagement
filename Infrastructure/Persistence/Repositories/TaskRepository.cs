using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class TaskRepository(AppDbContext _context) : ITaskRepository
    {
        public async System.Threading.Tasks.Task<Domain.Entities.Task> AddAsync(Domain.Entities.Task entity, CancellationToken cancellationToken)
        {
            await _context.Tasks.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async System.Threading.Tasks.Task UpdateAsync(Domain.Entities.Task entity, CancellationToken cancellationToken)
        {
            _context.Tasks.Update(entity);
            await System.Threading.Tasks.Task.CompletedTask;
        }

        public async System.Threading.Tasks.Task DeleteAsync(Domain.Entities.Task entity, CancellationToken cancellationToken)
        {
            _context.Tasks.Remove(entity);
            await System.Threading.Tasks.Task.CompletedTask;
        }

        public async System.Threading.Tasks.Task<Domain.Entities.Task?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public  IQueryable<Domain.Entities.Task> Query()
        {
            return _context.Tasks;
        }
        
    }
}
