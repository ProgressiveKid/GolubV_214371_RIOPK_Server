using CorporateRiskManagementSystemBack.Data;

namespace CorporateRiskManagementSystemBack.Application.Services
{
    public interface IUserService
    {
        int GetUserIdByName(string username);
    }
}
