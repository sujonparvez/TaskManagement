using MediatR;
using Domain.Interfaces;
using MassTransit;
using Application.DTOs;

namespace Application.Features.Tasks.Commands
{
    public class UpdateTaskCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public int? AssignedUserId { get; set; }
        public int TeamId { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, int>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
        }

        public async System.Threading.Tasks.Task<int> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with Id {request.Id} not found.");
            }

            task.Title = request.Title;
            task.Description = request.Description;
            task.Status = request.Status;
            task.AssignedUserId = request.AssignedUserId;
            task.TeamId = request.TeamId;
            task.DueDate = request.DueDate;

            await _taskRepository.UpdateAsync(task, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _publishEndpoint.Publish(new TaskUpdatedMessage(task), cancellationToken);

            return task.Id;
        }
    }
}
