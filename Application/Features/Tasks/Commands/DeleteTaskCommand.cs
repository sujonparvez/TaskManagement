using MediatR;
using Domain.Interfaces;

namespace Application.Features.Tasks.Commands
{
    public class DeleteTaskCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, int>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async System.Threading.Tasks.Task<int> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with Id {request.Id} not found.");
            }

            await _taskRepository.DeleteAsync(task, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return task.Id;
        }
    }
}
