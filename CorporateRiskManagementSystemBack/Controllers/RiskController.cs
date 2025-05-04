using CorporateRiskManagementSystemBack.Application.Interfaces;
using CorporateRiskManagementSystemBack.Application.Services;
using CorporateRiskManagementSystemBack.Domain.Entites;
using CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var userId = _userService.GetUserIdByName(User.Identity.Name);
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

            // Логика для создания риска
            // Например, сохранить в базе данных
            // _context.Risks.Add(new Risk { ... });

            return Ok(new { message = "Risk created successfully" });
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
