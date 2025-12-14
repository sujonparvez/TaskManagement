using Domain.Enums;

namespace Application.Interfaces
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        UserRole Role { get; }
    }
}
