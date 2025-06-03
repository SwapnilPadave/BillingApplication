using BA.Api.Infra.Extensions;
using BA.Api.Infra.Filters;
using BA.Utility.Constant;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace BA.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ActionFilter>();
            });

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.RegisterRepositories();
            builder.Services.RegisterServices();
            builder.Services.ConfigureDatabase(builder.Configuration);
            builder.Services.ConfigureCors(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(Constants.CORS_KEY);
            app.UseAuthentication();
            //app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
