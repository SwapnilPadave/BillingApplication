namespace BA.Service.Token
{
    public interface ITokenService
    {
        Task SaveTokenAsync(int userId, string jwtToken, DateTime expireAt, CancellationToken cancellationToken);
    }
}
