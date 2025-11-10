using AutoMapper;
using BA.Api.Infra.Authentication;
using BA.Api.Infra.MediatorHandlers.AuthHandler;
using BA.Api.Infra.Requests.LoginRequest;
using BA.Dtos.LoginDto;
using BA.Service.Login;
using BA.Service.Token;
using BA.Utility.Token;
using Microsoft.AspNetCore.Authorization;
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
    public class AuthController : BaseController
    {
        private readonly ILoginService _loginService;
        private readonly ITokenService _tokenService;
        private readonly JwtOptions _jwtOptions;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ILoginService loginService, ITokenService tokenService, IOptions<JwtOptions> jwtOptions, ILogger<AuthController> logger, IMapper mapper)
        {
            _loginService = loginService;
            _tokenService = tokenService;
            _jwtOptions = jwtOptions.Value;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<Dictionary<string, object>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var userData = await _loginService.GetLoginDetails(request.UserId, request.Password, cancellationToken);
            if (userData == null)
            {
                return APIResponse("BA504", null!);
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim("UserId", userData.UserId.ToString()),
                    new Claim("UserName", userData.UserName),
                    new Claim("IsActive", userData.IsActive.ToString().ToLower()),
                    new Claim("Admin", userData.Admin.ToString().ToLower()),
                    new Claim("Role", userData.Admin?"Admin":"User")
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
                var expireTime = DateTime.Now.AddMinutes(_jwtOptions.ExpiryMinutes);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                string refreshToken = TokenGenerator.GenerateRefreshToken();
                var refreshTokenExpire = DateTime.Now.AddMinutes(_jwtOptions.RefreshExpiryMinutes);

                await _tokenService.SaveTokenAsync(userData.UserId, tokenString, expireTime, refreshToken, refreshTokenExpire, cancellationToken);

                return APIResponse("BA100", new
                {
                    Token = tokenString,
                    Expiration = expireTime,
                    RefreshToken = refreshToken,
                    RefreshTokenExpireAt = refreshTokenExpire
                });
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

        [Authorize]
        [HttpPost("Logout")]
        public async Task<Dictionary<string, object>> Logout(CancellationToken cancellationToken)
        {
            ExtractUserContext();
            var result = await _loginService.Logout(UserId, cancellationToken);
            if (result.IsSuccess)
                return APIResponse("BA100", null!);
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("RefreshToken")]
        public async Task<Dictionary<string, object>> Refresh([FromBody] RefreshTokenRequest request)
        {
            var result = await _loginService.GetRefreshToken(request.RefreshToken, _jwtOptions.Key, _jwtOptions.ExpiryMinutes, _jwtOptions.Issuer, _jwtOptions.Audience);
            if (!result.IsSuccess)
            {
                return APIResponse(result.Error.ErrorMsg, null!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

    }
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
