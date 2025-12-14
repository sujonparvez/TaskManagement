using Application.DTOs;
using MediatR;
using Domain.Interfaces;
using System.Globalization;
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries
{
    public class GetAllUsersQuery : IRequest<PaginatedList<UserDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Id";
        public bool IsAscending { get; set; } = true;
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PaginatedList<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PaginatedList<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {

            var query = _userRepository.Query();

            query = request.SortBy.ToLower() switch
            {
                "fullname" => request.IsAscending ? query.OrderBy(u => u.FullName) : query.OrderByDescending(u => u.FullName),
                "email" => request.IsAscending ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email),
                _ => query.OrderBy(u => u.Id)
            };

            var count = await query.CountAsync(cancellationToken);
            var users = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.Role
                }).ToListAsync(cancellationToken);

            return new PaginatedList<UserDto>(users, count, request.PageNumber, request.PageSize);
        }
    }
}
