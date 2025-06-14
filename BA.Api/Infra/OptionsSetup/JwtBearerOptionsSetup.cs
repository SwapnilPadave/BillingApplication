using BA.Api.Infra.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BA.Api.Infra.OptionsSetup
{
    public class JwtBearerOptionsSetup(
        IOptions<JwtOptions> jwtOptions,
        ILogger<JwtBearerOptionsSetup> logger) : IConfigureOptions<JwtBearerOptions>
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;
        private readonly ILogger<JwtBearerOptionsSetup> _logger = logger;

        public void Configure(JwtBearerOptions options)
        {
            // 🔍 Log the values being used (for debugging only)
            _logger.LogInformation("🔐 JWT Validation Setup:");
            _logger.LogInformation("Issuer: {Issuer}", _jwtOptions.Issuer);
            _logger.LogInformation("Audience: {Audience}", _jwtOptions.Audience);
            _logger.LogInformation("ExpiryMinutes: {Minutes}", _jwtOptions.ExpiryMinutes);

            // ⚠️ WARNING: log the key only for local debugging, never in production
            _logger.LogWarning("🔑 JWT Key used (debug only): {Key}", _jwtOptions.Key);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key))
            };
        }
    }
}
