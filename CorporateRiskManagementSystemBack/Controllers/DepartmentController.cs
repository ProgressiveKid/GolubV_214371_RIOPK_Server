using CorporateRiskManagementSystemBack.Data;
using CorporateRiskManagementSystemBack.Domain.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CorporateRiskManagementSystemBack.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly RiskDbContext _context;
        private readonly IMemoryCache _cache;
        const string cacheKey = "departments_list";

        public DepartmentController(RiskDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet("OnGetDepartmentsAsync")]
        public async Task<IActionResult> OnGetDepartmentsAsync()
        {

            if (!_cache.TryGetValue(cacheKey, out List<object> departments))
            {
                departments = await _context.Departments
                                            .Select(d => new { d.DepartmentId, d.Name })
                                            .AsNoTracking()
                                            .Cast<object>() // приведение к object, чтобы тип совпадал
                                            .ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); // кэш живёт 10 минут

                _cache.Set(cacheKey, departments, cacheEntryOptions);
            }

            return new JsonResult(departments);
        }
    }
}
