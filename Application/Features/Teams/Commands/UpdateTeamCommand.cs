using MediatR;
using Domain.Interfaces;

namespace Application.Features.Teams.Commands
{
    public class UpdateTeamCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, int>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTeamCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetByIdAsync(request.Id, cancellationToken);
            if (team == null)
            {
                throw new KeyNotFoundException($"Team with Id {request.Id} not found.");
            }

            team.Name = request.Name;
            team.Description = request.Description;

            await _teamRepository.UpdateAsync(team, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return team.Id;
        }
    }
}
