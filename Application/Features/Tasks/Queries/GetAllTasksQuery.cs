using Application.DTOs;
using MediatR;
using Domain.Interfaces;
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Domain.Enums;
namespace Application.Features.Tasks.Queries
{
    public class GetAllTasksQuery : IRequest<PaginatedList<TaskDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Id";
        public bool IsAscending { get; set; } = true;

        public Domain.Enums.TaskStatus? Status { get; set; }
        public int? AssignedUserId { get; set; }
        public int? TeamId { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, PaginatedList<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository, ICurrentUserService currentUserService)
        {
            _taskRepository = taskRepository;
            _currentUserService = currentUserService;
        }

        public async System.Threading.Tasks.Task<PaginatedList<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            
            var query = _taskRepository.Query();

            if (request.Status.HasValue)
            {
                query = query.Where(t => t.Status == request.Status);
            }
            if (request.AssignedUserId.HasValue)
            {
                query = query.Where(t => t.AssignedUserId == request.AssignedUserId.Value);
            }
            if (request.TeamId.HasValue)
            {
                query = query.Where(t => t.TeamId == request.TeamId.Value);
            }
            if (request.DueDate.HasValue)
            {
                query = query.Where(t => t.DueDate.Date == request.DueDate.Value.Date);
            }
            if (_currentUserService.Role == UserRole.Employee)
            {
                int assignedUserId = _currentUserService.UserId;
                query = query.Where(t => t.AssignedUserId == assignedUserId);
            }

            query = request.SortBy.ToLower() switch
            {
                "title" => request.IsAscending ? query.OrderBy(t => t.Title) : query.OrderByDescending(t => t.Title),
                "status" => request.IsAscending ? query.OrderBy(t => t.Status) : query.OrderByDescending(t => t.Status),
                "duedate" => request.IsAscending ? query.OrderBy(t => t.DueDate) : query.OrderByDescending(t => t.DueDate),
                _ => query.OrderBy(t => t.Id)
            };

            var count = await query.CountAsync(cancellationToken);
            var tasks = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(task => new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    AssignedUserId = task.AssignedUserId,
                    AssignedUserName = task.AssignedUser != null ? task.AssignedUser.FullName : null,
                    CreatedByUserId = task.CreatedByUserId,
                    CreatedByUserName = task.CreatedByUser != null ? task.CreatedByUser.FullName : null,
                    TeamId = task.TeamId,
                    TeamName = task.Team != null ? task.Team.Name : null,
                    DueDate = task.DueDate
                })

                .ToListAsync(cancellationToken);

            return new PaginatedList<TaskDto>(tasks, count, request.PageNumber, request.PageSize);
        }
    }
}
