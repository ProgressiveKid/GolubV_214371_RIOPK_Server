using CorporateRiskManagementSystemBack.Data;
using CorporateRiskManagementSystemBack.Domain.Entites;

namespace CorporateRiskManagementSystemBack.Infrastructure.Repositories.Interfaces
{
    public interface IRiskRepository
    {
        public IEnumerable<Risk> GetAll();
    }
}
