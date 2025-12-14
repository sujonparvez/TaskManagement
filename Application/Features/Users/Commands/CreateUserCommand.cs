using MediatR;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Enums;
using Application.Interfaces;

namespace Application.Features.Users.Commands
{
    public class CreateUserCommand : IRequest<int>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork,IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var defaultPassword = _passwordHasher.Hash("Default123!");

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Role = request.Role,
                Password = defaultPassword,
            };

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
