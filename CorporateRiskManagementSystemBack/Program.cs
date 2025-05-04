using CorporateRiskManagementSystemBack.Application.Interfaces;
using CorporateRiskManagementSystemBack.Application.Services;
using CorporateRiskManagementSystemBack.Data;
using CorporateRiskManagementSystemBack.Infrastructure.Repositories;
using CorporateRiskManagementSystemBack.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
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
                options.AddPolicy("AllowLocalhost", policy =>
                {
                    policy.WithOrigins("https://localhost:7100")  // URL фронтенда
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();  // Это важно, чтобы куки передавались
                });
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "AuthCookie"; // ДОЛЖНО БЫТЬ ТОЧНО ТАКОЕ ЖЕ имя
                    options.Cookie.SameSite = SameSiteMode.None; // SameSite политики, чтобы куки работали в разных приложениях
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.Path = "/";
                });


            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<RiskDbContext>(options => options.UseNpgsql(connection)); // подключение к бд
            builder.Services.AddScoped<IRiskRepository, RiskRepository>();
            builder.Services.AddScoped<IRiskService, RiskService>();
            builder.Services.AddScoped<IUserService, UserService>();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // Используем CORS
            app.UseRouting();
            app.UseCors("AllowLocalhost");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "autorisationRoute",
                    pattern: "AutorisationF/Autorisation",
                    defaults: new { controller = "AuthController", action = "Autorisation" });
            });

            app.MapControllers();

            app.Run();
        }
    }
}
