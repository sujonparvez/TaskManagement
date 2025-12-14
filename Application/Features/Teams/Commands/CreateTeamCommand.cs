using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Features.Teams.Commands
{
    public class CreateTeamCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, int>
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTeamCommandHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = new Team
            {
                Name = request.Name,
                Description = request.Description
            };

            await _teamRepository.AddAsync(team, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return team.Id;
        }
    }
}
