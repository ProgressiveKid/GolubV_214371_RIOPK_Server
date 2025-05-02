using CorporateRiskManagementSystemBack.Application.Interfaces;
using CorporateRiskManagementSystemBack.Application.Services;
using CorporateRiskManagementSystemBack.Data;
using CorporateRiskManagementSystemBack.Infrastructure.Repositories;
using CorporateRiskManagementSystemBack.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CorporateRiskManagementSystemBack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Добавляем CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:7100")  // Разрешаем запросы с этого адреса
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<RiskDbContext>(options => options.UseNpgsql(connection)); // подключение к бд
            builder.Services.AddScoped<IRiskRepository, RiskRepository>();
            builder.Services.AddScoped<IRiskService, RiskService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // Используем CORS
            app.UseCors("AllowLocalhost");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
