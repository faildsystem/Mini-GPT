
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mini_GPT.Data;
using Mini_GPT.Interfaces;
using Mini_GPT.Models;
using Mini_GPT.Services;
using MongoDB.Driver;

namespace Mini_GPT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<AppSqlDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Configure MongoDB settings
            builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
            // Register MongoDbContext with the configuration
            builder.Services.AddSingleton<MongoDbContext>();
            // Register ChatService
            builder.Services.AddScoped<IChatService, ChatService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
