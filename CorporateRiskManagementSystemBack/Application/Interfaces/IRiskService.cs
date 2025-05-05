using CorporateRiskManagementSystemBack.Domain.Entites;

namespace CorporateRiskManagementSystemBack.Application.Interfaces
{
    public interface IRiskService
    {
        int CreateRisk(Risk risk);

        int CreateRiskAssessment(RiskAssessment riskAssessment);

        int UpdateRiskAssessment(RiskAssessment riskAssessment);

        int LinkRiskToDepartment(int idRisk, int idDepartment);
        List<Risk> CheckRisksAssessmentForDepartment(int idDepartment);
        List<Risk> GetAllRisks();
        List<Risk> GetRisksForDepartment(int departmentId);
        RiskAssessment GetAssessmentForRisk(int riskId);

    }
}
