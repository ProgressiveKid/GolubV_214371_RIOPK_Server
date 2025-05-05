using CorporateRiskManagementSystemBack.Application.Interfaces;
using CorporateRiskManagementSystemBack.Application.Services;
using CorporateRiskManagementSystemBack.Domain.Entites;
using CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace CorporateRiskManagementSystemBack.Controllers
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
                Console.WriteLine("asdsad");
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
            var assessment = _riskService.GetAssessmentForRisk(request.RiskId);
            var riskAssessment = new RiskAssessment()
            {
                AssessmentId = assessment.AssessmentId,
                RiskId = request.RiskId,
                AssessedById = userId,
                AssessmentDate = request.AssessmentDate,
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

            var riskAssessment = new RiskAssessment()
            {
                RiskId = request.RiskId,
                AssessedById = userId,
                AssessmentDate = request.AssessmentDate,
                ImpactScore = (short)request.ImpactScore,
                ProbabilityScore = (short)request.ProbabilityScore,
                Notes = request.Notes,
            };

            var createRiskAssessment = _riskService.CreateRiskAssessment(riskAssessment);
            return Ok(new { message = "Оценка успешно добавлена" });
        }

        // GET: RiskController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RiskController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RiskController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RiskController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RiskController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RiskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RiskController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RiskController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
