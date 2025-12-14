using Application.DTOs;
using MediatR;
using Domain.Interfaces;

namespace Application.Features.Tasks.Queries
{
    public class GetTaskByIdQuery : IRequest<TaskDto>
    {
        public int Id { get; set; }
    }

    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTaskByIdQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);
            if (task == null)
            {
                return null;
            }

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                AssignedUserId = task.AssignedUserId,
                AssignedUserName = task.AssignedUser != null ? task.AssignedUser.FullName : null,
                CreatedByUserId = task.CreatedByUserId,
                CreatedByUserName = task.CreatedByUser != null ? task.CreatedByUser .FullName: null,
                TeamId = task.TeamId,
                TeamName = task.Team != null ? task.Team.Name : null,
                DueDate = task.DueDate
            };
        }
    }
}
