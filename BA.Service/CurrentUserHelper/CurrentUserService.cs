using Microsoft.AspNetCore.Http;

namespace BA.Service.CurrentUserHelper
{
    public class CurrentUserService : ICurrentUserService
    {
        public int UserId { get; }
        public string? UserName { get; }
        public CurrentUserService(IHttpContextAccessor accessor)
        {
            var user = accessor?.HttpContext?.User;
            UserId = int.TryParse(user?.FindFirst("UserId")?.Value, out var id) ? id : 0;
            UserName = user?.FindFirst("UserName")?.Value ?? string.Empty;
        }
    }
}
