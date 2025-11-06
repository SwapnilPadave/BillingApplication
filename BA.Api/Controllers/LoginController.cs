using AutoMapper;
using BA.Api.Infra.Authentication;
using BA.Api.Infra.Requests.LoginRequest;
using BA.Dtos.LoginDto;
using BA.Service.Login;
using BA.Service.Token;
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
        private readonly ITokenService _tokenService;
        private readonly JwtOptions _jwtOptions;
        private readonly IMapper _mapper;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILoginService loginService,
                               IMapper mapper,
                               IOptions<JwtOptions> options,
                               ILogger<LoginController> logger,
                               ITokenService tokenService)
        {
            _loginService = loginService;
            _mapper = mapper;
            _jwtOptions = options.Value;
            _logger = logger;
            _tokenService = tokenService;
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
                        new Claim(ClaimTypes.NameIdentifier, userData.UserId.ToString()), // <-- fixed
                        new Claim(ClaimTypes.Name, userData.UserName),                   // optional
                        new Claim("IsActive", userData.IsActive.ToString().ToLower()),
                        new Claim("Admin", userData.Admin.ToString().ToLower()),
                        new Claim(ClaimTypes.Role, userData.Admin ? "Admin" : "User"),  // optional
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwtOptions.Issuer,
                    audience: _jwtOptions.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(_jwtOptions.ExpiryMinutes),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                await _tokenService.SaveTokenAsync(userData.UserId, tokenString, token.ValidTo, cancellationToken);

                return APIResponse("BA100", new { Token = tokenString, Expiration = token.ValidTo });
            }
            else
            {
                return APIFailureResponse("BA101", null!);
            }
        }

        [HttpPost("Register")]
        public async Task<Dictionary<string, object>> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var requetDto = _mapper.Map<RegisterUserDto>(request);
            var result = await _loginService.RegisterUserAsync(requetDto, cancellationToken);
            if (result.IsSuccess)
                return APIResponse("BA107", result.Data!);
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("Logout")]
        public async Task<Dictionary<string, object>> Logout(CancellationToken cancellationToken)
        {
            ExtractUserContext();
            var result = await _loginService.Logout(UserId, cancellationToken);
            if (result.IsSuccess)
                return APIResponse("BA503", null!);
            return APIFailureResponse(result.Error.ErrorMsg, null!);
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
