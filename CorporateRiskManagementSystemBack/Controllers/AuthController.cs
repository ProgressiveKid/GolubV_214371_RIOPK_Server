using System.Security.Claims;
using CorporateRiskManagementSystemBack.Data;
using CorporateRiskManagementSystemBack.Domain.Entites;
using CorporateRiskManagementSystemBack.Domain.Entites.Enums;
using CorporateRiskManagementSystemBack.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorporateRiskManagementSystemBack.Controllers
{
    public class AuthController : Controller
    {
        RiskDbContext db;
        public AuthController(RiskDbContext context)
        {
            db = context;
        }

        [HttpGet("Autorisation")]
        public async Task<IActionResult> Autorisation()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Ваш код, который будет выполнен, если пользователь авторизован
                //var rooms = await db.Rooms.ToListAsync();
                return View();
            }
            else
            {
                // Ваш код, который будет выполнен, если пользователь не авторизован
                return View("Autorisation");
                // Пример перенаправления на страницу входа
            }
        }

        [HttpPost("Autorisation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Autorisation([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Ищем пользователя по email и паролю
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.PasswordHash == model.Password);
                if (user != null)
                {
                    Role curRole = Enum.Parse<Role>(user.Role);
                    // Здесь нужно будет выполнить аутентификацию, например, создать сессию или токен JWT
                    await Authenticate(model.Email, curRole);

                    // Ответ в формате JSON
                    return Json(new { success = true, redirectUrl = "/Home/Index" });
                }
                else
                {
                    // Если пользователь не найден, отправляем ошибку
                    return Json(new { success = false, message = "Некорректные логин и(или) пароль" });
                }
            }

            // Если модель невалидна
            return Json(new { success = false, message = "Неверные данные" });
        }

        [HttpGet("Registration")]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost("Registration")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration([FromForm] RegisterViewModel userData)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == userData.Email);
                var role = Role.Auditor.ToString();
                if (user == null)
                {
                    // добавляем пользователя в бд
                    db.Users.Add(new User
                    {
                        Email = userData.Email,
                        PasswordHash = userData.Password,
                        Role = role,
                        FullName = userData.FirstName,
                        Username = userData.LastName,
                    });
                    await db.SaveChangesAsync();
                    await Authenticate(userData.Email, Role.Auditor); // аутентификация
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(userData);
        }

        private async Task Authenticate(string userName, Role role)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role.ToString())
            };
            // создаем объект ClaimsIdentity
            // установка аутентификационных куки
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id), new AuthenticationProperties
            {
                IsPersistent = true, // или false в зависимости от ваших требований
                ExpiresUtc = DateTimeOffset.UtcNow.AddYears(1) // установите желаемый срок действия
            });
        }
    }
}
