
using CorporateRiskManagementSystemBack.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Xunit;
using CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels;
using CorporateRiskManagementSystemBack.Domain.Entites;
using CorporateRiskManagementSystemBack;

namespace TestCRMS.Integrations
{
    public class ReportControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public ReportControllerIntegrationTests()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Здесь нужно использовать InMemoryDatabase для тестов
                        var connection = "InMemoryDbForTesting"; // строка для имитации базы данных в памяти
                        services.AddDbContext<RiskDbContext>(options =>
                            options.UseInMemoryDatabase(connection));
                    });
                });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task CreateReport_ReturnsBadRequest_WhenAnyRiskHasNoAssessment()
        {
            // Arrange: создаём риски и департамент с необходимыми данными
            var scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<RiskDbContext>();

            // Начинаем транзакцию
            var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var userId = dbContext.Users.FirstOrDefault().UserId;
                // Создаём риски
                var department = new Department {  Name = "FinanceForTests" };
                dbContext.Departments.Add(department);

                var risk1 = new Risk
                {
                    Title = "Risk 1",
                    CreatedById = userId,
                    CreatedAt = DateTime.Now,
                    Description = "wake up on buggati",
                    Severity = "High",  // Установите значение для поля severity
                    Likelihood = "Low",
                };

                var risk2 = new Risk
                {
                    Title = "Risk 2",
                    CreatedById = userId,
                    Severity = "High",
                    CreatedAt = DateTime.Now,
                    Description = "wake up on buggati",
                    Likelihood = "Low",
                };

                dbContext.Risks.Add(risk1);
                dbContext.Risks.Add(risk2);
                dbContext.SaveChanges();

                var request = new CreateReportRequest
                {
                    Username = "admin",
                    DepartmentId = department.DepartmentId,
                    Content = "Контент"
                };

                var response = await _client.PostAsJsonAsync("/Report/CreateReport", request);

                // Assert: проверка на BadRequest с нужным сообщением
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                var content = await response.Content.ReadAsStringAsync();
                content.Should().Be("Необходимо выполнить оценку всех существующих рисков для отдела");
            }
            finally
            {
                // Откатываем все изменения, сделанные в рамках транзакции
                await transaction.RollbackAsync();
            }
        }
    }
}
