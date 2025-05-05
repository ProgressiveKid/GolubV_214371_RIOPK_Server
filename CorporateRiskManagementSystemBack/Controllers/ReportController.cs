using CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels;
using CorporateRiskManagementSystemBack.Domain.Entites;
using Microsoft.AspNetCore.Mvc;
using CorporateRiskManagementSystemBack.Data;
using CorporateRiskManagementSystemBack.Application.Interfaces;

namespace CorporateRiskManagementSystemBack.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        IRiskService _riskService;
        RiskDbContext db;
        public ReportController(RiskDbContext db, IRiskService riskService)
        {
            this.db = db;
            _riskService = riskService;
        }

        [HttpPost("CreateReport")]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportRequest request)
        {
            var departmentRisks = _riskService.GetRisksForDepartment(request.DepartmentId);
            if (departmentRisks.Any(u => u.IsHaveAssessment))
            {
                return BadRequest("Необходимо выполнить оценку всех существующих рисков для отдела");
            }

            return Ok(new { message = "Risk created successfully" });
        }

        [HttpGet("CanReportBuild")]
        public async Task<JsonResult> CanReportBuild([FromQuery] int departmentId)
        {
            var departmentRisks = _riskService.GetRisksForDepartment(departmentId);

            return Json(departmentRisks);
        }

        [HttpGet("GetReports")]
        public async Task<IActionResult> GetReport([FromQuery] int departmentId)
        {


            return Ok(new { message = "Risk created successfully" });
        }
    }
}
