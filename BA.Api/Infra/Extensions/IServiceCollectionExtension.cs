using BA.Api.Infra.Filters;
using BA.Api.Infra.Validators.UserValidations;
using BA.Database;
using BA.Database.Infra;
using BA.Database.Repos.BillRepository;
using BA.Database.Repos.CustomerBillDetailsRepository;
using BA.Database.Repos.CustomerRepository;
using BA.Database.Repos.EmailTemplateRepository;
using BA.Database.Repos.EmployeeRepository;
using BA.Database.Repos.GeneratedBillRepository;
using BA.Database.Repos.NewsPapersReposiotry;
using BA.Database.Repos.TokenRepository;
using BA.Database.Repos.UserRepository;
using BA.Database.Repos.UsersRepository;
using BA.Service;
using BA.Utility.Constant;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
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
            services.AddTransient(typeof(ICustomerDetailsRepository), typeof(CustomerDetailsRepository));
            services.AddTransient(typeof(IBillRepository), typeof(BillRepository));
            services.AddTransient(typeof(IEmployeeRepository), typeof(EmployeeRepository));
            services.AddTransient(typeof(ITokenRepository), typeof(TokenRepository));
            services.AddTransient(typeof(ICustomerBillDetailsRepository), typeof(CustomerBillDetailsRepository));
            services.AddTransient(typeof(IGeneratedBillDetailsRepository), typeof(GeneratedBillDetailsRepository));
            services.AddTransient(typeof(IEmailTemplateRepository), typeof(EmailTemplateRepository));

            services.AddTransient<SqlCommands>();
            services.AddTransient<SqlServiceHelper>();
            services.AddTransient<DapperServiceHelper>();
            //services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddTransient(typeof(FluentValidationActionFilter<>));
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
            services.AddScoped(typeof(FluentValidationActionFilter<>));
            services.AddSingleton<IFilterProvider, FluentValidationFilterProvider>();
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
                        //.WithOrigins(urls!)
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
        }
    }
}
