using BA.Database.Infra;
using BA.Entities.Token;

namespace BA.Database.Repos.TokenRepository
{
    public class TokenRepository : Repository<JwtToken>, ITokenRepository
    {
        public TokenRepository(BAContext context) : base(context)
        {
        }
    }
}
