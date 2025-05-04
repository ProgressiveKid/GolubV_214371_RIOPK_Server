using CorporateRiskManagementSystemBack.Application.Interfaces;
using CorporateRiskManagementSystemBack.Data;
using CorporateRiskManagementSystemBack.Domain.Entites;
using CorporateRiskManagementSystemBack.Infrastructure.Repositories.Interfaces;

namespace CorporateRiskManagementSystemBack.Infrastructure.Repositories
{
    public class RiskRepository : IRiskRepository
    {
        RiskDbContext db;
        public RiskRepository(RiskDbContext db)
        {
            this.db = db;
        }
        public int LinkRiskToDepartment(int idRisk, int idDepartment)
        {
            var risk = db.Risks.FirstOrDefault(u => u.RiskId == idRisk);
            if (risk == null)
            {
                return -1;
            }
            var department = db.Departments.FirstOrDefault(d => d.DepartmentId == idDepartment);
            if (department == null)
            {
                return -1;
            }

            risk.Departments.Add(department);
            // Сохранение изменений
            db.SaveChanges();
            return risk.RiskId;
        }

        public int CreateNewRisk(Risk risk)
        {
            db.Risks.Add(risk);
            db.SaveChanges();
            return risk.RiskId;
        }
        public Risk Get(int id)
        {
            Risk risk = db.Risks.Where(u => u.RiskId == id).First();
            return risk;
        }
        public List<Risk> GetRisksForDepartment(int departmentId)
        {
            var risks = db.Risks
                          .Where(r => r.Departments.Any(d => d.DepartmentId == departmentId))
                          .ToList();
            return risks;
        }
        public IEnumerable<Risk> GetAll()
        {
            var risk = db.Risks.ToList();
            return risk;
        }
    }
}
