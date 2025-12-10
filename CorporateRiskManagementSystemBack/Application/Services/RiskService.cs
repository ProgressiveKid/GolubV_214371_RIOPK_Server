using CorporateRiskManagementSystemBack.Application.Interfaces;
using CorporateRiskManagementSystemBack.Domain.Entites;
using CorporateRiskManagementSystemBack.Infrastructure.Repositories.Interfaces;

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

        public Risk? GetRiskById(int riskId)
        {
            return _riskRepository.GetRiskById(riskId);
        }

        public int DeleteRisk(int riskId)
        {
            return _riskRepository.DeleteRisk(riskId);
        }

        public int CreateRiskAssessment(RiskAssessment riskAssessment) => _riskRepository.CreateRiskAssessment(riskAssessment);

        public int LinkRiskToDepartment(
            int idRisk,
            int idDepartment)
            => _riskRepository.LinkRiskToDepartment(idRisk, idDepartment);

        public List<Risk> GetAllRisks() => _riskRepository.GetAll().ToList();

        public List<Risk> GetRisksForDepartment(int departmentId) {
            return _riskRepository.GetRisksForDepartment(departmentId);
        }

        public List<Risk> CheckRisksAssessmentForDepartment(int departmentId)
        {
            return _riskRepository.CheckRisksAssessmentForDepartment(departmentId);
        }

        public RiskAssessment GetAssessmentForRisk(int riskId)
        {
            return _riskRepository.GetAssessmentByRiskId(riskId);
        }

        public int UpdateRiskAssessment(RiskAssessment riskAssessment)
        {
            return _riskRepository.UpdateRiskAssessment(riskAssessment);
        }
    }
}
