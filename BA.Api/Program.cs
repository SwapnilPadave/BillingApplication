using BA.Api.Infra.Authentication;
using BA.Api.Infra.Extensions;
using BA.Api.Infra.Filters;
using BA.Api.Infra.Middleware;
using BA.Database.Repos.TokenRepository;
using BA.Utility.AppSettings;
using BA.Utility.Constant;
using BA.Utility.Content;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using MediatR;

namespace BA.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var assembly = typeof(Program).Assembly;

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            QuestPDF.Settings.License = LicenseType.Community;

            ContentLoader.LanguageLoader(Directory.GetCurrentDirectory());
            //builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ActionFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });


            //builder.Services.AddAllFluentValidators();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            //builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

            builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            builder.Services.RegisterRepositories();
            builder.Services.RegisterServices();
            builder.Services.ConfigureDatabase(builder.Configuration);
            //builder.Services.AddAllFluentValidators();
            builder.Services.ConfigureCors(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAllFluentValidators();

            builder.Services.AddScoped(typeof(FluentValidationActionFilter<>));
            builder.Services.AddValidatorsFromAssembly(assembly);
            builder.Services.AddSingleton<IFilterProvider, FluentValidationFilterProvider>();

            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddJWTAuthentication(builder.Configuration);
            builder.Services.AddSwaggerWithJwtSupport();

            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(Constants.SMTPSETTINGS_KEY));

            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(Constants.JWT_KEY));

            var jwtSettings = builder.Configuration.GetSection(Constants.JWT_KEY).Get<JwtOptions>();

            #region MyRegion
            builder.Services.AddAuthentication(options =>
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

                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        // Get user ID from token claims
                        var userIdClaim = context.Principal?.FindFirst(ClaimTypes.NameIdentifier);
                        if (userIdClaim == null)
                        {
                            context.Fail("Invalid token: UserId missing.");
                            return;
                        }

                        var userId = int.Parse(userIdClaim.Value);

                        // Resolve your repository to check IsActive
                        var tokenRepo = context.HttpContext.RequestServices.GetRequiredService<ITokenRepository>();
                        var userToken = await tokenRepo.GetAsync(userId);

                        if (userToken == null || !userToken.IsActive)
                        {
                            context.Fail("Token is inactive.");
                        }
                    }
                };
            });
            #endregion



            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BA API V1");
                    //c.RoutePrefix = string.Empty;
                });
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(Constants.CORS_KEY);
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
