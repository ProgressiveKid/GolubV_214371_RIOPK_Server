using CorporateRiskManagementSystemBack.Data;
using CorporateRiskManagementSystemBack.Domain.Entites;

namespace CorporateRiskManagementSystemBack.Infrastructure.Repositories.Interfaces
{
    public interface IRiskRepository
    {
        int CreateNewRisk(Risk risk);
        int CreateRiskAssessment(RiskAssessment riskAssessment);

        int LinkRiskToDepartment(int idRisk, int departmentId);
        IEnumerable<Risk> GetAll();
        List<Risk> GetRisksForDepartment(int departmentId);
        List<Risk> CheckRisksAssessmentForDepartment(int departmentId);
        RiskAssessment GetAssessmentByRiskId(int riskId);
        int UpdateRiskAssessment(RiskAssessment riskAssessment);
    }
}
