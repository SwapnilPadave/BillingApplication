using BA.Api.Infra.Authentication;
using BA.Utility.Constant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace BA.Api.Infra.Extensions
{
    public static class JwtExtensions
    {
        #region Commented for not used
        //public static void AddSwaggerWithJwtSupport(this IServiceCollection services)
        //{
        //    services.AddSwaggerGen(c =>
        //    {
        //        c.SwaggerDoc("v1", new OpenApiInfo { Title = "AspNetCore", Version = "v1" });

        //        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        //        {
        //            Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
        //              Enter 'Bearer' [space] and then your token in the text input below.
        //              \r\n\r\nExample: 'Bearer 12345abcdef'",
        //            Name = "Authorization",
        //            In = ParameterLocation.Header,
        //            Type = SecuritySchemeType.ApiKey,
        //            Scheme = "Bearer"
        //        });
        //        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        //          {
        //              {
        //                  new OpenApiSecurityScheme
        //                  {
        //                      Reference = new OpenApiReference
        //                      {
        //                          Type = ReferenceType.SecurityScheme,
        //                          Id = "Bearer"
        //                      },
        //                      Scheme = "Bearer",
        //                      Name = "Authorization",
        //                      In = ParameterLocation.Header
        //                  },
        //                  new List<string>()
        //              }
        //          });
        //        c.OperationFilter<AuthorizeCheckOperationFilter>();
        //    });
        //}

        //public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var jwtSettings = configuration.GetSection(Constants.JWT_KEY).Get<JwtOptions>();
        //    services.AddAuthentication(options =>
        //    {
        //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    })
        //    .AddJwtBearer(options =>
        //    {
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidateLifetime = true,
        //            ValidateIssuerSigningKey = true,
        //            ValidIssuer = jwtSettings?.Issuer,
        //            ValidAudience = jwtSettings?.Audience,
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),

        //            NameClaimType = ClaimTypes.Name,
        //            RoleClaimType = ClaimTypes.Role
        //        };

        //        options.Events = new JwtBearerEvents
        //        {
        //            OnTokenValidated = async context =>
        //            {
        //                // Get user ID from token claims
        //                var userIdClaim = context.Principal?.FindFirst(ClaimTypes.NameIdentifier);
        //                if (userIdClaim == null)
        //                {
        //                    context.Fail("Invalid token: UserId missing.");
        //                    return;
        //                }

        //                var userId = int.Parse(userIdClaim.Value);

        //                // Resolve your repository to check IsActive
        //                var tokenRepo = context.HttpContext.RequestServices.GetRequiredService<ITokenRepository>();
        //                var userToken = await tokenRepo.GetAsync(userId);

        //                if (userToken == null || !userToken.IsActive)
        //                {
        //                    context.Fail("Token is inactive.");
        //                }
        //            }
        //        };
        //    });
        //}

        //public static void AddSwaggerAuthentication(this IServiceCollection services, string secret)
        //{
        //    var key = Encoding.ASCII.GetBytes(secret);
        //    services.AddAuthentication(x =>
        //    {
        //        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    })
        //    .AddJwtBearer(x =>
        //    {
        //        x.RequireHttpsMetadata = false;
        //        x.SaveToken = true;
        //        x.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false,
        //            ValidateAudience = false
        //        };
        //    });
        //} 
        #endregion

        public static void AddSwaggerWithJwtAuthenticationSupport(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter valid generated token without Bearer."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection(Constants.JWT_KEY).Get<JwtOptions>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings?.Issuer,
                    ValidAudience = jwtSettings?.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                    NameClaimType = "UserName",
                    RoleClaimType = "Role",
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"❌ JWT validation failed: {context.Exception.Message}");
                        if (context.Exception.InnerException != null)
                            Console.WriteLine($"   → Inner: {context.Exception.InnerException.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        //Console.WriteLine("✅ JWT validated successfully.");
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
