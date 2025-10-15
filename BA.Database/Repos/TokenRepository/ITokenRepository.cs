using BA.Database.Infra;
using BA.Entities.Token;

namespace BA.Database.Repos.TokenRepository
{
    public interface ITokenRepository : IRepository<JwtToken>
    {
    }
}
