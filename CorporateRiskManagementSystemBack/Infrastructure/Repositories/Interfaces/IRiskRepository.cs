using CorporateRiskManagementSystemBack.Data;
using CorporateRiskManagementSystemBack.Domain.Entites;

namespace CorporateRiskManagementSystemBack.Infrastructure.Repositories.Interfaces
{
    public interface IRiskRepository
    {
        int CreateNewRisk(Risk risk);
        int LinkRiskToDepartment(int idRisk, int idDepartment);
        IEnumerable<Risk> GetAll();
        List<Risk> GetRisksForDepartment(int departmentId);
    }
}
