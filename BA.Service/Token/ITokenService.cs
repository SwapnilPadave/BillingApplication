namespace BA.Service.Token
{
    public interface ITokenService
    {
        Task SaveTokenAsync(int userId, string jwtToken, DateTime expireAt, string refreshToken, DateTime refreshTokenExpireAt, CancellationToken cancellationToken);
    }
}
