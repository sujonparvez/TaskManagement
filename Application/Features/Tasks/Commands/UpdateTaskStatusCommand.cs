using MediatR;
using Domain.Interfaces;
using Application.Interfaces;

namespace Application.Features.Tasks.Commands
{
    public class UpdateTaskStatusCommand : IRequest<int>
    {
        public int Id { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }

    }

    public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, int>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private ITaskNotifier _notifier;

        public UpdateTaskStatusCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork,ICurrentUserService currentUserService, ITaskNotifier notifier)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _notifier = notifier;
        }

        public async System.Threading.Tasks.Task<int> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with Id {request.Id} not found.");
            }

            if(_currentUserService.UserId != task.AssignedUserId)
            {
                throw new Exception($"Not allowed to update");
            }

            task.Status = request.Status;

            await _taskRepository.UpdateAsync(task, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (task.Status == Domain.Enums.TaskStatus.Done)
            {
                await _notifier.NotifyTaskCompletedAsync(task.AssignedUserId.Value, task.Id, task.Title, cancellationToken);
            }
            

            return task.Id;
        }
    }
}
