using CorporateRiskManagementSystemBack.Infrastructure.Data;

namespace CorporateRiskManagementSystemBack.Application.Services
{
    public class UserService : IUserService
    {
        RiskDbContext _db;

        public UserService(RiskDbContext db)
        {
            _db = db;
        }

        public int GetUserIdByName(string username) => _db.Users.FirstOrDefault(u => u.Username == username).UserId;

        public int GetUserIdByEmail(string email) => _db.Users.FirstOrDefault(u => u.Email == email).UserId;

    }
}
