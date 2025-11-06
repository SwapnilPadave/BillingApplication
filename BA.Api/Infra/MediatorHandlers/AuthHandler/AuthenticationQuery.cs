using BA.Api.Infra.Authentication;
using BA.Database;
using BA.Dtos.LoginDto;
using BA.Utility;
using BA.Utility.Content;
using BA.Utility.Result;
using BA.Utility.Token;
using Dapper;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BA.Api.Infra.MediatorHandlers.AuthHandler
{
    public class AuthenticationQuery : IRequest<Result>
    {
        public string UserId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public class AuthenticationQueryHandler : IRequestHandler<AuthenticationQuery, Result>
        {
            private readonly DapperServiceHelper _dapper;
            private readonly JwtOptions _jwtOptions;
            public AuthenticationQueryHandler
                (
                DapperServiceHelper dapper,
                IOptions<JwtOptions> jwtOptions
                )
            {
                _dapper = dapper;
                _jwtOptions = jwtOptions.Value;
            }
            public async Task<Result> Handle(AuthenticationQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var encryptedPassword = Utils.Encrypt(request.Password);

                    var parameterForGetLogin = new DynamicParameters();
                    parameterForGetLogin.Add("@UserId", request.UserId);
                    parameterForGetLogin.Add("@Password", encryptedPassword);

                    var result = await _dapper.QueryFirstOrDefaultAsync<GetLoginDetails>("USP_GetUserLoginDetails", parameterForGetLogin);
                    if (result == null)
                    {
                        return Result.Failure(new Error("BA504"));
                    }
                    else
                    {
                        var claims = new List<Claim>
                        {
                            new Claim("UserId", result.UserId.ToString()),
                            new Claim("UserName", result.UserName),
                            new Claim("IsActive", result.IsActive.ToString().ToLower()),
                            new Claim("Admin", result.Admin.ToString().ToLower()),
                            new Claim("Role", result.Admin?"Admin":"User")
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

                        var parameterForSaveToken = new DynamicParameters();
                        parameterForSaveToken.Add("@UserId", result.UserId);
                        parameterForSaveToken.Add("@Token", tokenString);
                        parameterForSaveToken.Add("@ExpireAt", expireTime);
                        parameterForSaveToken.Add("@RefreshToken", refreshToken);
                        parameterForSaveToken.Add("@RefreshTokenExpireAt", refreshTokenExpire);

                        await _dapper.ExecuteAsync("Usp_InsertTokenDetails", parameterForSaveToken);

                        return Result.Success(new 
                        { 
                            Token = tokenString, 
                            Expiration = expireTime,
                            RefreshToken = refreshToken,
                            RefreshTokenExpireAt = refreshTokenExpire
                        });
                    }
                }
                catch (Exception ex)
                {
                    return Result.Failure(new Error(ex.Message));
                }
            }
        }
    }
}
