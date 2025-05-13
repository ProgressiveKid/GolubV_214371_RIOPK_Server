using CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels;
using CorporateRiskManagementSystemBack.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CorporateRiskManagementSystemBack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        RiskDbContext _dbContext;

        public AdminController(RiskDbContext dbContext)
        { 
            _dbContext = dbContext;
        }

        [HttpGet("GetUserName")]
        public async Task<IActionResult> GetUserName()
        {

            List<string> usersList = await _dbContext.Users
                                     .Select(user => $"{user.FullName} {user.Username} {user.Role} {user.Email} ")
                                     .ToListAsync();

            return new JsonResult(usersList);
        }

        [HttpGet("GetUserIdByEmail")]
        public async Task<IActionResult> GetUserIdByEmail([FromQuery] string email)
        {
            var user = await _dbContext.Users
                .Where(u => u.Email == email)
                .Select(u => new { u.UserId })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound("Пользователь не найден");

            return new JsonResult(user);
        }

        [HttpPost("ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeRoleRequest request)
        {
            var user = await _dbContext.Users.FindAsync(request.UserId);
            if (user == null)
                return NotFound("Пользователь не найден");

            if (!new[] { "Auditor", "Administrator", "Manager" }.Contains(request.NewRole))
                return BadRequest("Недопустимая роль");

            user.Role = request.NewRole;
            await _dbContext.SaveChangesAsync();

            return Ok("Роль успешно обновлена");
        }
        // получить всех пользователей
        // изменить роль пользователю
        // изменить пользователю данные
    }
}
