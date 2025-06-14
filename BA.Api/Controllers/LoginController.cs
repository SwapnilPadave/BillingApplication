using AutoMapper;
using BA.Api.Infra.Authentication;
using BA.Api.Infra.Requests.LoginRequest;
using BA.Dtos.LoginDto;
using BA.Service.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        private readonly ILoginService _loginService;
        private readonly JwtOptions _jwtOptions;
        private readonly IMapper _mapper;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILoginService loginService,
                               IMapper mapper,
                               IOptions<JwtOptions> options,
                               ILogger<LoginController> logger)
        {
            _loginService = loginService;
            _mapper = mapper;
            _jwtOptions = options.Value;
            _logger = logger;
        }

        [HttpPost("Login")]
        public async Task<Dictionary<string, object>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("🔐 Login request received for UserId: {UserId}", request.UserId);
            _logger.LogInformation("🔑 JWT Key used for signing (debug only): {Key}", _jwtOptions.Key);

            var userData = await _loginService.GetLoginDetails(request.UserId, request.Password, cancellationToken);

            if (userData != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("UserId", userData.UserId.ToString()),
                    new Claim("UserName", userData.UserName),
                    //new Claim("MobileNumber", userData.MobileNumber ?? ""),
                    //new Claim("EmailAddress", userData.EmailAddress ?? ""),
                    new Claim("IsActive", userData.IsActive.ToString().ToLower()),
                    new Claim("Admin", userData.Admin.ToString().ToLower())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwtOptions.Issuer,
                    audience: _jwtOptions.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return APIResponse("BA100", new { Token = tokenString, Expiration = token.ValidTo });
            }
            else
            {
                return APIResponse("BA101", null!);
            }
        }

        [HttpPost("Register")]
        public async Task<Dictionary<string, object>> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var requetDto = _mapper.Map<RegisterUserDto>(request);
            var result = await _loginService.RegisterUserAsync(requetDto, cancellationToken);
            if (result.IsSuccess)
                return APIResponse("BA107", result.Data!);
            return APIResponse(result.Error.ErrorMsg, null!);
        }

        [HttpGet("GetInfo")]
        public Dictionary<string, object> GetInfo()
        {
            ExtractUserContext();
            return APIResponse("BA100", new
            {
                UserId,
                UserName,
                IsAdmin,
                IsActive
            });
        }
    }
}
