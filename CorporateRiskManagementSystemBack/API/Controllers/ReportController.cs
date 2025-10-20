using CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels;
using CorporateRiskManagementSystemBack.Domain.Entites;
using Microsoft.AspNetCore.Mvc;
using CorporateRiskManagementSystemBack.Application.Interfaces;
using CorporateRiskManagementSystemBack.Application.Services;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using CorporateRiskManagementSystemBack.Infrastructure.Data;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout.Properties;
using Newtonsoft.Json;
using iText.IO.Font;
using iText.Kernel.Geom;
using Path = System.IO.Path;
using Org.BouncyCastle.Utilities;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout.Borders;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Xobject;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace CorporateRiskManagementSystemBack.API.Controllers
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
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest("Описание отчета не может быть пустым");                
            }
            var departament = db.Departments.FirstOrDefault(x => x.DepartmentId == request.DepartmentId);

            var report = new AuditReport
            {
                AuthorId = userId,
                CreatedAt = DateTime.Now,
                Content = request.Content,
                Title = $"Аудиторский отчет от: {DateTime.Now.ToShortDateString()} по подразделению {departament.Name}",
                DepartmentId = request.DepartmentId,
            };

            string username = request.Username;
            var user = db.Users.FirstOrDefault(u => u.Email == username);
            if (user == null)
                return BadRequest("Не найден автор отчёта");

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string reportsFolderPath = Path.Combine(documentsPath, "Reports");
            Directory.CreateDirectory(reportsFolderPath);
            string pdfPath = Path.Combine(reportsFolderPath, $"{username}_doc.pdf");
            byte[] pdfBytes; // Объявляем переменную для хранения байтов PDF
            string fontPath = Path.Combine(Directory.GetCurrentDirectory(), "Properties", "ARIAL.TTF");
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Properties", "logo.png");

            using (var memoryStream = new MemoryStream())
            using (var writer = new PdfWriter(memoryStream))  // ТОЛЬКО ОДИН writer!
            using (var pdf = new PdfDocument(writer))
            using (var document = new Document(pdf))
            {

                // Настройка шрифта
                try
                {
                    PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
                    document.SetFont(font);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при установке шрифта: {ex.Message}");
                }

                // Заголовок и логотип
                var container = new Div().SetKeepTogether(true);
                container.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                DateTime dateTime = DateTime.Now;

                if (System.IO.File.Exists(imagePath))
                {
                    Image img = new Image(ImageDataFactory.Create(imagePath))
                        .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                    // Задать максимальные размеры (ширина и высота) для изображения
                    float maxWidth = 200f;  // Максимальная ширина
                    float maxHeight = 100f; // Максимальная высота

                    // Уменьшаем изображение пропорционально, чтобы оно вписалось в указанные размеры
                    img.ScaleToFit(maxWidth, maxHeight);

                    // Получаем размеры страницы
                    PageSize pageSize = pdf.GetDefaultPageSize();
                    float pageWidth = pageSize.GetWidth();
                    float pageHeight = pageSize.GetHeight();

                    // Координаты: правый верхний угол с отступом
                    float x = pageWidth - img.GetImageScaledWidth() - 20;
                    float y = pageHeight - img.GetImageScaledHeight() - 20;

                    // Установка абсолютной позиции и прозрачности
                    img.SetFixedPosition(1, x, y); // страница 1, координаты
                    //img.SetOpacity(0.3f); // 30% прозрачность
                    container.Add(img);
                }
                document.Add(new Paragraph($"Аудиторский отчет от: {DateTime.Now.ToShortDateString()}"));
                document.Add(new Paragraph($"По подразделению: {departament.Name}"));

               // document.Add(container);

                document.Add(new Paragraph($"Уникальный идентификатор пользователя: {userId}"));
                document.Add(new Paragraph($"ФИО аудитора: {user.FullName}"));
                document.Add(new Paragraph($"Электронная почта: {user.Email}"));

                // Создаем таблицу с нужным количеством столбцов
                float[] columnWidths = { 1, 2, 1, 2, 2, 2};  // Количество столбцов и их ширина (относительная, в части от всей ширины страницы)

                Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
                // Загружаем шрифт для емодзи
                string emojiFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "seguiemj.ttf");
                PdfFont emojiFont = PdfFontFactory.CreateFont(emojiFontPath, PdfEncodings.IDENTITY_H);
                // Устанавливаем таблицу на всю ширину
                // Устанавливаем столбцы с заданной шириной
                // Добавляем заголовки столбцов
                table.AddCell("Риск ID");
                table.AddCell("Название");
                table.AddCell("Вероятность");
                table.AddCell("Серьёзность");
                table.AddCell("Оценка влияния");
                table.AddCell("Оценка вероятности");

                // Перебираем все риски и добавляем их в таблицу
                foreach (var risk in departmentRisks)
                {
                    var departmentRisksAssessment = _riskService.GetAssessmentForRisk(risk.RiskId);

                    // Добавляем данные для каждого риска в строку таблицы
                    table.AddCell(risk.RiskId.ToString());
                    table.AddCell(risk.Title);
                    table.AddCell(risk.Likelihood.ToString());
                    table.AddCell(risk.Severity.ToString());
                    var impactScore = string.Empty;
                    for (int i = 0; i < Convert.ToInt64(departmentRisksAssessment.ImpactScore); i++)
                    {
                        impactScore += "🔥"; 
                    }
                    // 🔥 Ячейка только с огнём и emoji-шрифтом
                    Paragraph fireEmoji = new Paragraph(impactScore).SetFont(emojiFont).SetFontSize(10);
                    table.AddCell(new Cell().Add(fireEmoji));
                    var probabilityScore = string.Empty;
                    for (int i = 0; i < Convert.ToInt64(departmentRisksAssessment.ProbabilityScore); i++)
                    {
                        probabilityScore += "🎲";
                    }
                    Paragraph cubeEmoji = new Paragraph(probabilityScore).SetFont(emojiFont).SetFontSize(10);
                    table.AddCell(new Cell().Add(cubeEmoji));

                }

                // Добавляем таблицу в документ
                document.Add(table);
                document.Add(new Paragraph($"Заключение аудитора: {request.Content}"));
                document.Close();
                // Получаем байты PDF из memory stream
                document.Flush();
                pdfBytes = memoryStream.ToArray();
            }
            await SaveReportToDatabase(report, pdfBytes);
            return Ok("Отчёт успешно создан и сохранён в 'Мои документы/Reports'");

        }
        private async Task SaveReportToDatabase(AuditReport report, byte[] pdfBytes)
        {
            try
            {
                report.PdfReport = pdfBytes;
                db.AuditReports.Add(report);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Логируем ошибку, но не прерываем основной поток
                Console.WriteLine($"Ошибка при сохранении отчета в БД: {ex.Message}");
            }
        }

        [HttpGet("CanReportBuild")]
        public async Task<JsonResult> CanReportBuild([FromQuery] int departmentId)
        {
            var canBeReportBuild = _riskService.GetRisksForDepartment(departmentId)
                        .TrueForAll(x => x.IsHaveAssessment);
            return Json(canBeReportBuild);
        }

        [HttpGet("DownloadReport/{reportId}")]
        public IActionResult DownloadReport(int reportId)
        {
            var report = db.AuditReports.FirstOrDefault(r => r.ReportId == reportId);

            if (report == null || report.PdfReport == null)
                return NotFound("Отчет не найден");

            // Проверяем размер файла (должен быть больше 1KB)
            if (report.PdfReport.Length < 1024)
            {
                return BadRequest("PDF файл поврежден или слишком мал");
            }

            // Возвращаем PDF файл
            return File(report.PdfReport, "application/pdf", $"report_{reportId}.pdf");
        }

        [HttpGet("GetReports/{departmentsId}")]
        public List<AuditReport> GetReportsByDepartment(int departmentsId)
        {
            var reports = db.AuditReports.Where(r => r.DepartmentId == departmentsId).ToList();

            // Возвращаем PDF файл
            return reports;
        }
        [HttpPost("DeleteReport/{reportId}")]
        public async Task<IActionResult> DeleteReport(int reportId)
        {
            try
            {
                var report = await db.AuditReports.FirstOrDefaultAsync(r => r.ReportId == reportId);

                if (report == null)
                {
                    return NotFound(new { message = "Отчет не найден" });
                }

                db.AuditReports.Remove(report);
                await db.SaveChangesAsync();

                return Ok(new { message = "Отчет успешно удален" });
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Console.WriteLine($"Ошибка при удалении отчета {reportId}: {ex.Message}");
                return StatusCode(500, new { message = "Произошла ошибка при удалении отчета" });
            }
        }

        [HttpGet("ViewReport/{reportId}")]
        public IActionResult ViewReport(int reportId)
        {
            var report = db.AuditReports.FirstOrDefault(r => r.ReportId == reportId);

            if (report == null || report.PdfReport == null)
                return NotFound("Отчет не найден");

            // Показать в браузере (не скачивать)
            return File(report.PdfReport, "application/pdf");
        }
    }
}
