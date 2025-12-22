using CorporateRiskManagementSystemBack.Application.Interfaces;
using CorporateRiskManagementSystemBack.Application.Services;
using CorporateRiskManagementSystemBack.Domain.Entites;
using CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels;
using CorporateRiskManagementSystemBack.Domain.Entites.Enums;
using CorporateRiskManagementSystemBack.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorporateRiskManagementSystemBack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RiskController : Controller
    {
        IRiskService _riskService;
        IUserService _userService;
        RiskDbContext _dbContext;
        public RiskController(            IRiskService riskService,            IUserService userService,            RiskDbContext dbContext)        {
            _riskService = riskService;
            _userService = userService;
            _dbContext = dbContext;
        }


        [HttpGet("GetAllRisks")]
        public JsonResult GetAllRisks()
        {
            var aa = User.Identity;
            if (User.IsInRole("Auditor"))
            {
                Console.WriteLine("Hello");
            }
            var allRisks = _riskService.GetAllRisks();
            return Json(allRisks);
        }

        [HttpGet("GetAllRisksTypes")]
        public JsonResult GetAllRisksTypes()
        {
            var allRiskTypes = Enum.GetNames(typeof(RiskType));
            return Json(allRiskTypes);
        }

        [HttpPost("CreateRisk")]
        public async Task<IActionResult> CreateRisk([FromBody] CreateRiskRequest request)
        {
            if (request == null)
            {
                return BadRequest("Пустые данные");
            }

            var userId = _userService.GetUserIdByEmail(request.UsernameId);
            if (userId == 0)
            {
                return BadRequest("Пользователь с авторизоавнным юзернейном не найден");

            }
            RiskType riskType;
            if (!Enum.TryParse<RiskType>(request.RiskType, true, out riskType))
            {
                riskType = RiskType.Operational;
            }
            var newRisk = new Risk()
            {
                CreatedById = userId,
                CreatedAt = DateTime.Now,
                Title = request.Title,
                RiskType = riskType,
                Description = request.Description,
                Likelihood = request.Likelihood,
                Severity = request.Severity,
            };

            var createdRiskId = _riskService.CreateRisk(newRisk);
            var initialStatus = new Status
            {
                RiskId = newRisk.RiskId,
                StatusName = RiskStatuses.NoAssessment,
                StatusDescription = "Аудитор создал риск - ожидаем оценки",
                ChangedById = userId,
                ChangedAt = DateTime.Now
            };

            _dbContext.Statuses.Add(initialStatus);
            var linkedDepartment = _riskService.LinkRiskToDepartment(createdRiskId, request.DepartmentId);

            return Ok(new { message = "Risk created successfully" });
        }

        [HttpGet("GetRisksForDepartment")]
        public async Task<JsonResult> GetRisksForDepartment([FromQuery] int departmentId)
        {
            var departmentRisks = _riskService.GetRisksForDepartment(departmentId);
            return Json(departmentRisks);
        }

        [HttpGet("GetAssessmentForRisk")]
        public async Task<JsonResult> GetAssessmentForRisk([FromQuery] int riskId)
        {
            var assessment = _riskService.GetAssessmentForRisk(riskId);

            return Json(assessment);
        }

        [HttpPut("EditAssessment")]
        public async Task<IActionResult> EditAssessment([FromBody] RiskAssessmentRequest request)
        {
            var userId = _userService.GetUserIdByName(request.UsernameId);
            if (userId == 0)
            {
                return BadRequest("Пользователь с авторизоавнным юзернейном не найден");

            }
            var dateTimeUtc = new DateTime(request.AssessmentDate.Year, request.AssessmentDate.Month, request.AssessmentDate.Day, 0, 0, 0, DateTimeKind.Utc);

            var assessment = _riskService.GetAssessmentForRisk(request.RiskId);
            var riskAssessment = new RiskAssessment()
            {
                AssessmentId = assessment.AssessmentId,
                RiskId = request.RiskId,
                AssessedById = userId,
                AssessmentDate = dateTimeUtc,
                ImpactScore = (short)request.ImpactScore,
                ProbabilityScore = (short)request.ProbabilityScore,
                Notes = request.Notes,
            };
            var allStatuses = _dbContext.Statuses.Where(x => x.RiskId == request.RiskId).ToList();
            var updatedAssessment = _riskService.UpdateRiskAssessment(riskAssessment);
            return Json(updatedAssessment);
        }

        [HttpGet("CheckRisksAssessmentForDepartment")]
        public async Task<JsonResult> CheckRisksAssessmentForDepartment([FromQuery] int departmentId)
        {
            List<Risk> departmentsRisks = _riskService.GetRisksForDepartment(departmentId).ToList();
            var countNeddedAssesments = departmentsRisks.Where(x => !x.IsHaveAssessment).ToList();
            return Json(countNeddedAssesments.Count());
        }

        [HttpPost("AddAssessments")]
        public async Task<IActionResult> AddAssessments([FromBody] RiskAssessmentRequest request)
        {
            var userId = _userService.GetUserIdByName(request.UsernameId);
            if (userId == 0)
            {
                return BadRequest("Пользователь с авторизоавнным юзернейном не найден");

            }
            var dateTimeUtc = new DateTime(request.AssessmentDate.Year, request.AssessmentDate.Month, request.AssessmentDate.Day, 0, 0, 0, DateTimeKind.Utc);
            var riskAssessment = new RiskAssessment()
            {
                RiskId = request.RiskId,
                AssessedById = userId,
                AssessmentDate = dateTimeUtc,
                ImpactScore = (short)request.ImpactScore,
                ProbabilityScore = (short)request.ProbabilityScore,
                Notes = request.Notes,
            };

            var createRiskAssessment = _riskService.CreateRiskAssessment(riskAssessment);
            var assessmentStatus = new Status
            {
                RiskId = request.RiskId,
                StatusName = RiskStatuses.AssessmentCompleted,
                StatusDescription = "Аудитор произвёл оценку риска",
                ChangedById = userId,
                ChangedAt = DateTime.Now
            };

            try
            {
                _dbContext.Statuses.Add(assessmentStatus);
                var changes = _dbContext.SaveChanges(); // Вернет количество измененных строк
                Console.WriteLine($"Saved {changes} entities"); // Должно быть >= 1
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException?.Message;
                // Логируйте ошибку
            }
            return Ok(new { message = "Оценка успешно добавлена" });
        }

        [HttpDelete("DeleteRisk")]
        public async Task<IActionResult> DeleteRisk([FromQuery] int riskId)
        {
            try
            {
                // Проверяем существует ли риск
                var risk = _riskService.GetRiskById(riskId);
                if (risk == null)
                {
                    return NotFound(new { message = "Риск не найден" });
                }

                var assessment = _riskService.GetAssessmentForRisk(riskId);

                // Удаляем риск и связанные оценки
                var result = _riskService.DeleteRisk(riskId);

                if (result == 1)
                {
                    return Ok(new { message = "Риск и связанные оценки успешно удалены" });
                }
                else
                {
                    return StatusCode(500, new { message = "Ошибка при удалении риска" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ошибка при удалении риска: {ex.Message}" });
            }
        }

        [HttpGet("GetRisksWithStatus")]
        public async Task<IActionResult> GetRisksWithStatus([FromQuery] int departmentId)
        {
            var r2isks = await _dbContext.Risks
                .Where(r => r.Departments.Any(d => d.DepartmentId == departmentId))
                .Include(r => r.CreatedBy)
                .Include(r => r.Departments)
                .Include(r => r.RiskAssessments)
                    .ThenInclude(ra => ra.AssessedBy)
                .Include(r => r.Statuses)  // Include после Where
                    .ThenInclude(s => s.ChangedBy)
                .ToListAsync();

            var risks = await _dbContext.Risks
                .Include(r => r.CreatedBy)
                .Include(r => r.Departments)
                .Include(r => r.RiskAssessments)
                    .ThenInclude(ra => ra.AssessedBy)
                .Include(r => r.Statuses)
                    .ThenInclude(s => s.ChangedBy)
                .Where(r => r.Departments.Any(d => d.DepartmentId == departmentId))
                .Select(r => new
                {
                    r.RiskId,
                    r.Title,
                    r.Description,
                    r.Severity,
                    r.Likelihood,
                    r.RiskType,
                    r.CreatedAt,
                    CreatedBy = r.CreatedBy.FullName,
                    Departments = r.Departments.Select(d => new { d.DepartmentId, d.Name }),
                    RiskAssessments = r.RiskAssessments.Select(ra => new
                    {
                        ra.AssessmentId,
                        ra.ImpactScore,
                        ra.ProbabilityScore,
                        ra.Notes,
                        ra.AssessmentDate,
                        AssessedBy = ra.AssessedBy != null ? new
                        {
                            ra.AssessedBy.FullName,
                            ra.AssessedBy.UserId
                        } : null
                    }).ToList(),

                    // Текущий статус (последний по дате)
                    CurrentStatus = r.Statuses
                        .OrderByDescending(s => s.ChangedAt)
                        .Select(s => new
                        {
                            StatusName = s.StatusName,
                            StatusDescription = s.StatusDescription,
                            ChangedAt = s.ChangedAt,
                            ChangedBy = s.ChangedBy.FullName
                        })
                        .FirstOrDefault() ?? (new
                        {
                            StatusName = "Оценка отсутствует",
                            StatusDescription = "Статус не установлен",
                            ChangedAt = r.CreatedAt,
                            ChangedBy = r.CreatedBy.FullName
                        }),

                    // История статусов
                    StatusHistory = r.Statuses
                        .OrderByDescending(s => s.ChangedAt)
                        .Select(s => new
                        {
                            s.StatusName,
                            s.StatusDescription,
                            s.ChangedAt,
                            ChangedBy = s.ChangedBy.FullName
                        })
                        .ToList()
                })
                .ToListAsync();

            var result = risks.Select(r => new
            {
                r.RiskId,
                r.Title,
                r.Description,
                r.Severity,
                r.Likelihood,
                r.RiskType,
                r.CreatedAt,
                r.CreatedBy,
                r.Departments,
                r.RiskAssessments,
                r.CurrentStatus,
                r.StatusHistory,
                IsHaveAssessment = r.RiskAssessments.Count > 0
            });

            return Ok(result);
        }

        // GET: RiskController
        [HttpGet("Index")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
