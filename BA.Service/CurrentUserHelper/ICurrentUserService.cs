namespace BA.Service.CurrentUserHelper
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string? UserName { get; }
    }
}
