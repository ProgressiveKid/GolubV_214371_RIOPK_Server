using CorporateRiskManagementSystemBack.Data;
using CorporateRiskManagementSystemBack.Domain.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorporateRiskManagementSystemBack.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly RiskDbContext _context;

        public DepartmentController(RiskDbContext context)
        {
            _context = context;
        }

        [HttpGet("OnGetDepartmentsAsync")]
        public async Task<IActionResult> OnGetDepartmentsAsync()
        {
            var departments = await _context.Departments
                                            .Select(d => new { d.DepartmentId, d.Name })
                                            .ToListAsync();

            return new JsonResult(departments);
        }
    }
}
