using CorporateRiskManagementSystemBack.Application.Interfaces;
using CorporateRiskManagementSystemBack.Domain.Entites;
using CorporateRiskManagementSystemBack.Infrastructure.Repositories;
using CorporateRiskManagementSystemBack.Infrastructure.Repositories.Interfaces;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace CorporateRiskManagementSystemBack.Application.Services
{
    public class RiskService : IRiskService
    {
        IRiskRepository _riskRepository;

        public RiskService(IRiskRepository riskRepository)
        {
            _riskRepository = riskRepository;
        }
        public void CreateRisk(int i)
        {
            Console.WriteLine();
        }

        public List<Risk> GetAllRisks() => _riskRepository.GetAll().ToList();

        // Потом всё по подразделению
    }
}
