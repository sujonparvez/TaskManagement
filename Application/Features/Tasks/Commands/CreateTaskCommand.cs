using MediatR;
using Domain.Entities;
using Domain.Interfaces;
using Application.Interfaces;
using MassTransit;
using Application.DTOs;

namespace Application.Features.Tasks.Commands
{
    public class CreateTaskCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public int? AssignedUserId { get; set; }
        public int TeamId { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ITaskNotifier _notifier;
        public CreateTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IPublishEndpoint publishEndpoint, ITaskNotifier notifier)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _publishEndpoint = publishEndpoint;
            _notifier = notifier;
        }

        public async System.Threading.Tasks.Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new Domain.Entities.Task
            {
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                AssignedUserId = request.AssignedUserId,
                CreatedByUserId = _currentUserService.UserId,
                TeamId = request.TeamId,
                DueDate = request.DueDate
            };

            await _taskRepository.AddAsync(task, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _publishEndpoint.Publish(new TaskCreatedMessage(task), cancellationToken);

            if (task.AssignedUserId.HasValue)
            {
                await _notifier.NotifyTaskAssignedAsync(task.AssignedUserId.Value, task.Id, task.Title, cancellationToken);
            }

            return task.Id;
        }
    }
}
