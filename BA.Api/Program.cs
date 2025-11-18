using Asp.Versioning;
using BA.Api.Infra.Authentication;
using BA.Api.Infra.Extensions;
using BA.Api.Infra.Filters;
using BA.Api.Infra.Middleware;
using BA.Utility.AppSettings;
using BA.Utility.Constant;
using BA.Utility.Content;
using BA.Utility.Folder;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Infrastructure;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;

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

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ActionFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            //For api versioning
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            builder.Services.AddHttpContextAccessor();
            builder.Services.RegisterRepositories();
            builder.Services.RegisterServices();
            builder.Services.ConfigureDatabase(builder.Configuration);
            builder.Services.ConfigureCors(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAllFluentValidators();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerWithJwtAuthenticationSupport();
            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(Constants.SMTPSETTINGS_KEY));

            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(Constants.JWT_KEY));

            var jwtSettings = builder.Configuration.GetSection(Constants.JWT_KEY).Get<JwtOptions>();

            //For loging errors details to file
            var path = Path.Combine(Directory.GetCurrentDirectory(), FolderLocation.LOG_FOLDER, "app-log-.txt");
            Log.Logger = new LoggerConfiguration().WriteTo.File(path, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30, shared: true).CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BA API V1");
                });
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(Constants.CORS_KEY);

            #region For jwt header debug purpose
            //app.Use(async (context, next) =>
            //{
            //    var authHeader = context.Request.Headers["Authorization"].ToString();
            //    Console.WriteLine($"Authorization header: {authHeader}");
            //    await next.Invoke();
            //}); 
            #endregion

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
