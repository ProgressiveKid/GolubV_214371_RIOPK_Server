using CorporateRiskManagementSystemBack.Domain.Entites;

namespace CorporateRiskManagementSystemBack.Application.Interfaces
{
    public interface IRiskService
    {
        void CreateRisk(int id);

        List<Risk> GetAllRisks();
    }
}
