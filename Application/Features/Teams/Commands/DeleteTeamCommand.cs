using MediatR;
using Domain.Interfaces;

namespace Application.Features.Teams.Commands
{
    public class DeleteTeamCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, int>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTeamCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetByIdAsync(request.Id, cancellationToken);
            if (team == null)
            {
                throw new KeyNotFoundException($"Team with Id {request.Id} not found.");
            }

            await _teamRepository.DeleteAsync(team, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return team.Id;
        }
    }
}
