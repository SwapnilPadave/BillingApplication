using BA.Api.Infra.Authentication;
using BA.Api.Infra.Validators.UserValidations;
using BA.Database;
using BA.Database.Infra;
using BA.Database.Repos.NewsPapersReposiotry;
using BA.Database.Repos.UserRepository;
using BA.Database.Repos.UsersRepository;
using BA.Service;
using BA.Utility.Constant;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NetCore.AutoRegisterDi;
using System.Reflection;

namespace BA.Api.Infra.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            var assembliesToScan = new[]
            {
                Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(IBaseService))
            };

            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces();
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IUserRepository), typeof(UserRepository));
            services.AddTransient(typeof(IUserLoginMappingRepository), typeof
                (UserLoginMappingRepository));
            services.AddTransient(typeof(INewsPaperRepository), typeof(NewsPaperRepository));
            services.AddTransient<SqlCommands>();
            services.AddScoped<IJwtProvider, JwtProvider>();
        }

        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BAContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(Constants.SQL_CONNECTION_STRING_KEY))
                        .EnableSensitiveDataLogging(true);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static IServiceCollection AddAllFluentValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<UserValidator>();
            return services;
        }

        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration.GetSection(Constants.APP_SETTINGS_KEY).GetValue<string>(Constants.CLIENT_APP_URL_KEY);

            string[]? urls = origins?.Split(",", StringSplitOptions.RemoveEmptyEntries);

            services.AddCors(c =>
            {
                c.AddPolicy(Constants.CORS_KEY, builder =>
                    {
                        builder
                        .WithOrigins(urls!)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
        }
    }
}
