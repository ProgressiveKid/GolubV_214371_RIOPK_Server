using CorporateRiskManagementSystemBack.Application.Interfaces;
using CorporateRiskManagementSystemBack.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CorporateRiskManagementSystemBack.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RiskController : Controller
    {
        IRiskService _riskService;

        public RiskController(IRiskService riskService)        {
            _riskService = riskService;
        }


        [HttpGet("GetAllRisks")]
        public JsonResult GetAllRisks()
        {
            var allRisks = _riskService.GetAllRisks();
            return Json(allRisks);
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
