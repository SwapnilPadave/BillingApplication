using BA.Api.Infra.Authentication;
using BA.Utility.Constant;
using Microsoft.Extensions.Options;

namespace BA.Api.Infra.OptionsSetup
{
    public class JwtOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
    {
        private readonly IConfiguration _configuration = configuration;
        public void Configure(JwtOptions options)
        {
            _configuration.GetSection(Constants.JWT_KEY).Bind(options);
        }
    }
}
