using BA.Dtos.LoginDto;
using BA.Entities.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BA.Api.Infra.Authentication
{
    public interface IJwtProvider
    {
        string Generate(GetLoginDetails user);
    }
    public sealed class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;

        public string Generate(GetLoginDetails user)
        {
            var claims = new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_options.Key)),
                    SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                null,
                DateTime.UtcNow.AddHours(1),
                signingCredentials
                );

            string tokenValue = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return tokenValue;
        }
    }
}
