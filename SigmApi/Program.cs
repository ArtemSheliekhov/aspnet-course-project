
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SigmApi.Models.Contexts;
using SigmApi.Services;
using System.Text.Json;

namespace SigmApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IGamesService, GamesService>();
            builder.Services.AddScoped<ISteamService, SteamService>();


            builder.Services.AddDbContext<SteamContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SteamContext") ?? throw new InvalidOperationException("Connection string 'SteamContext' not found.")));

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}