using Application.DTOs;
using MediatR;
using Domain.Interfaces;
using System.Globalization;
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Teams.Queries
{
    public class GetAllTeamsQuery : IRequest<PaginatedList<TeamDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Id";
        public bool IsAscending { get; set; } = true;
    }

    public class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, PaginatedList<TeamDto>>
    {
        private readonly ITeamRepository _teamRepository;

        public GetAllTeamsQueryHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<PaginatedList<TeamDto>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
        {
            var query = _teamRepository.Query();

            query = request.SortBy.ToLower() switch
            {
                "name" => request.IsAscending ? query.OrderBy(t => t.Name) : query.OrderByDescending(t => t.Name),
                "description" => request.IsAscending ? query.OrderBy(t => t.Description) : query.OrderByDescending(t => t.Description),
                _ => query.OrderBy(t => t.Id)
            };

            var count = await query.CountAsync(cancellationToken);
            var teams = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(team => new TeamDto
                {
                    Id = team.Id,
                    Name = team.Name,
                    Description = team.Description
                }).ToListAsync(cancellationToken);

            return new PaginatedList<TeamDto>(teams, count, request.PageNumber, request.PageSize);
        }
    }
}
