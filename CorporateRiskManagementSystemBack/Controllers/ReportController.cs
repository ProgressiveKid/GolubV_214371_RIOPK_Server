using CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels;
using CorporateRiskManagementSystemBack.Domain.Entites;
using Microsoft.AspNetCore.Mvc;
using CorporateRiskManagementSystemBack.Data;
using CorporateRiskManagementSystemBack.Application.Interfaces;
using CorporateRiskManagementSystemBack.Application.Services;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace CorporateRiskManagementSystemBack.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        RiskDbContext db;
        IRiskService _riskService;
        IUserService _userService;
        public ReportController(RiskDbContext db, IRiskService riskService, IUserService userService)
        {
            this.db = db;
            _riskService = riskService;
            _userService = userService;
        }

        [HttpPost("CreateReport")]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportRequest request)
        {
            var departmentRisks = _riskService.GetRisksForDepartment(request.DepartmentId);
            if (departmentRisks.TrueForAll(u => !u.IsHaveAssessment))
            {
                return BadRequest("Необходимо выполнить оценку всех существующих рисков для отдела");
            }
            var userId = _userService.GetUserIdByName(request.Username);
            if (userId == 0)
            {
                return BadRequest("Пользователь с авторизоавнным юзернейном не найден");

            }

            var report = new AuditReport
            {
                AuthorId = userId,
                CreatedAt = DateTime.Now,
                Content = request.Content,
                DepartmentId = request.DepartmentId,
            };

            // Сохраняем документ в память
            using (MemoryStream ms = new MemoryStream())
            {
                // Создаем новый документ PDF
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Проверяем, что content не пустое
                if (string.IsNullOrWhiteSpace(request.Content))
                {
                    return BadRequest("Content cannot be empty.");
                }

                // Создаем шрифт для документа
                var font = FontFactory.GetFont("Arial", 12, Font.NORMAL);

                // Добавляем текст в документ PDF
                var content = request.Content;

                // Проверяем, помещается ли весь текст на странице
                document.Add(new Paragraph(content, font));

                // Закрываем документ после добавления содержимого
                document.Close();

                // Сохраняем документ в поток памяти
                return File(ms.ToArray(), "application/pdf", "report.pdf");
            }
        }

        [HttpGet("CanReportBuild")]
        public async Task<JsonResult> CanReportBuild([FromQuery] int departmentId)
        {
            var canBeReportBuild = _riskService.GetRisksForDepartment(departmentId)
                        .TrueForAll(x => x.IsHaveAssessment);
            return Json(canBeReportBuild);
        }

        [HttpGet("GetReports")]
        public async Task<IActionResult> GetReport([FromQuery] int departmentId)
        {


            return Ok(new { message = "Risk created successfully" });
        }
    }
}
