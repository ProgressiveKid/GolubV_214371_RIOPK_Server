using CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels;
using CorporateRiskManagementSystemBack.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            user.Role = request.NewRole;
            await _dbContext.SaveChangesAsync();

            return Ok("Роль успешно обновлена");
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById([FromQuery] int id)
        {
            var user = await _dbContext.Users
                .Where(u => u.UserId == id)
                .Select(u => new
                {
                    u.UserId,
                    u.FullName,
                    u.Username,
                    u.Email,
                    u.Role
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound("Пользователь не найден");

            return new JsonResult(user);
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest request)
        {
            var user = await _dbContext.Users.FindAsync(request.UserId);
            if (user == null)
                return NotFound("Пользователь не найден");

            // Проверяем уникальность email, если он изменился
            if (user.Email != request.Email)
            {
                var existingUser = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email && u.UserId != request.UserId);

                if (existingUser != null)
                    return BadRequest("Пользователь с таким email уже существует");
            }

            // Проверяем уникальность username, если он изменился
            if (user.Username != request.Username)
            {
                var existingUser = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Username == request.Username && u.UserId != request.UserId);

                if (existingUser != null)
                    return BadRequest("Пользователь с таким именем уже существует");
            }

            // Обновляем данные
            user.FullName = request.FullName;
            user.Username = request.Username;
            user.Email = request.Email;
            user.Role = request.Role;

            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok("Данные пользователя успешно обновлены");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при обновлении пользователя: {ex.Message}");
            }
        }
    }
}
