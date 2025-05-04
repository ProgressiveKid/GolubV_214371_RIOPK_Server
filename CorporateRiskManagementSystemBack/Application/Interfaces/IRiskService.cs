using CorporateRiskManagementSystemBack.Domain.Entites;

namespace CorporateRiskManagementSystemBack.Application.Interfaces
{
    public interface IRiskService
    {
        int CreateRisk(Risk risk);

        int LinkRiskToDepartment(int idRisk, int idDepartment);
        List<Risk> GetAllRisks();
        List<Risk> GetRisksForDepartment(int departmentId);

    }
}
