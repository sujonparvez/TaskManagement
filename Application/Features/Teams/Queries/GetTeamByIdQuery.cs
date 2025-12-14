using Application.DTOs;
using MediatR;
using Domain.Interfaces;

namespace Application.Features.Teams.Queries
{
    public class GetTeamByIdQuery : IRequest<TeamDto>
    {
        public int Id { get; set; }
    }

    public class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, TeamDto>
    {
        private readonly ITeamRepository _teamRepository;

        public GetTeamByIdQueryHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<TeamDto> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetByIdAsync(request.Id, cancellationToken);
            if (team == null)
            {
                return null;
            }

            return new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description
            };
        }
    }
}
