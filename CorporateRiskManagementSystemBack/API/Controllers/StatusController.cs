using CorporateRiskManagementSystemBack.Domain.Entites;
using CorporateRiskManagementSystemBack.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CorporateRiskManagementSystemBack.Application.Services;

namespace CorporateRiskManagementSystemBack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly RiskDbContext _dbContext;
        private readonly IUserService _userService;

        public StatusController(RiskDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        [HttpPost("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeStatusRequest request)
        {
            var userId = _userService.GetUserIdByName(request.Username);
            if (userId == null)
                return Unauthorized("Пользователь не найден");



            // Проверяем существование риска
            var riskExists = await _dbContext.Risks.AnyAsync(r => r.RiskId == request.RiskId);
            if (!riskExists)
                return NotFound($"Риск с ID {request.RiskId} не найден");

            // Создаем новую запись в истории статусов
            var newStatus = new Status
            {
                RiskId = request.RiskId,
                StatusName = request.NewStatus,
                StatusDescription = request.Description,
                ChangedById = userId,
                ChangedAt = DateTime.Now
            };

            _dbContext.Statuses.Add(newStatus);
            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                newStatus.StatusId,
                Message = "Статус успешно изменен"
            });
        }
    }

    public class ChangeStatusRequest
    {
        public int RiskId { get; set; }
        public string NewStatus { get; set; } = null!;
        public string? Description { get; set; }

        public string? Username { get; set; }
    }
}
