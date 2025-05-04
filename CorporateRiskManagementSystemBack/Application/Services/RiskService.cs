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
        public int CreateRisk(Risk risk) => _riskRepository.CreateNewRisk(risk);
        public int LinkRiskToDepartment(
            int idRisk,
            int idDepartment)
            => _riskRepository.LinkRiskToDepartment(idRisk, idDepartment);

        public List<Risk> GetAllRisks() => _riskRepository.GetAll().ToList();
    }
}
