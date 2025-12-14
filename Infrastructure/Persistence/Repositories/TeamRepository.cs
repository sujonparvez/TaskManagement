using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class TeamRepository(AppDbContext _context) : ITeamRepository
    {
        public async System.Threading.Tasks.Task<Team> AddAsync(Team entity, CancellationToken cancellationToken)
        {
            await _context.Teams.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async System.Threading.Tasks.Task UpdateAsync(Team entity, CancellationToken cancellationToken)
        {
            _context.Teams.Update(entity);
            await System.Threading.Tasks.Task.CompletedTask;
        }

        public async System.Threading.Tasks.Task DeleteAsync(Team entity, CancellationToken cancellationToken)
        {
            _context.Teams.Remove(entity);
            await System.Threading.Tasks.Task.CompletedTask;
        }

        public async System.Threading.Tasks.Task<Team?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Teams.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public IQueryable<Domain.Entities.Team> Query()
        {
            return _context.Teams;
        }
        public async System.Threading.Tasks.Task<IEnumerable<Team>> GetAllAsync(int pageNumber, int pageSize, string sortBy, bool isAscending, CancellationToken cancellationToken)
        {
            var query = _context.Teams.AsQueryable();

            query = sortBy.ToLower() switch
            {
                "name" => isAscending ? query.OrderBy(t => t.Name) : query.OrderByDescending(t => t.Name),
                "description" => isAscending ? query.OrderBy(t => t.Description) : query.OrderByDescending(t => t.Description),
                _ => query.OrderBy(t => t.Id)
            };

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
