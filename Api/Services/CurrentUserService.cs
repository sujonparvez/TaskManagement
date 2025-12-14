using Application.Interfaces;
using Domain.Enums;
using System.Security.Claims;

namespace Api.Services
{
    public class CurrentUserService(IHttpContextAccessor _httpContextAccessor) : ICurrentUserService
    {
        public int UserId
        {
            get
            {
                var idClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (idClaim != null && int.TryParse(idClaim.Value, out int userId))
                {
                    return userId;
                }
                return 0;
            }
        }

        public UserRole Role
        {
            get
            {
                var roleClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role);
                if (roleClaim != null && UserRole.TryParse(roleClaim.Value, out UserRole role))
                {
                    return role;
                }
                return UserRole.Employee;
            }
        }
    }
}
