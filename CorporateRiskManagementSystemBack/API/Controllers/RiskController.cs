using CorporateRiskManagementSystemBack.Application.Interfaces;
using CorporateRiskManagementSystemBack.Application.Services;
using CorporateRiskManagementSystemBack.Domain.Entites;
using CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace CorporateRiskManagementSystemBack.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RiskController : Controller
    {
        IRiskService _riskService;
        IUserService _userService;
        public RiskController(IRiskService riskService, IUserService userService)        {
            _riskService = riskService;
            _userService = userService;
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

        [HttpPost("CreateRisk")]
        public async Task<IActionResult> CreateRisk([FromBody] CreateRiskRequest request)
        {
            if (request == null)
            {
                return BadRequest("Пустые данные");
            }

            var userId = _userService.GetUserIdByName(request.UsernameId);
            if (userId == 0)
            {
                return BadRequest("Пользователь с авторизоавнным юзернейном не найден");

            }
            var newRisk = new Risk()
            {
                CreatedById = userId,
                CreatedAt = DateTime.Now,
                Title = request.Title,
                Description = request.Description,
                Likelihood = request.Likelihood,
                Severity = request.Severity,
            };

            var createdRiskId = _riskService.CreateRisk(newRisk);
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
            return Ok(new { message = "Оценка успешно добавлена" });
        }

        // GET: RiskController
        [HttpGet("Index")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
