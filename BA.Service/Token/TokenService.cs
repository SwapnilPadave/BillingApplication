using BA.Database.Infra;
using BA.Entities.Token;

namespace BA.Service.Token
{
    public class TokenService : ITokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SaveTokenAsync(int userId, string jwtToken, DateTime expireAt, CancellationToken cancellationToken)
        {
            try
            {
                var token = new JwtToken
                {
                    UserId = userId,
                    Token = jwtToken,
                    ExpireAt = expireAt,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                await _unitOfWork.TokenRepository.AddAsync(token);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
