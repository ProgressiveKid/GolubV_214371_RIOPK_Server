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
            // db.RiskDe
            return 1;
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

        public IEnumerable<Risk> GetAll()
        {
            //var risk = db.Risks.ToList();
            var risk = new List<Risk>();
            return risk;
        }
    }
}
