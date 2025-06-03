using Microsoft.AspNetCore.Mvc;
using _1.Data;
using _1.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _1.Utils;

namespace _1.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGDBContext _context;

        public AccountController(IGDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login);

            if (user != null)
            {
                bool isPasswordValid = PasswordHasher.VerifyPassword(password, user.Password);

                // Временная проверка на старый пароль (без хэша)
                bool isOldPasswordValid = false;
                if (!isPasswordValid)
                {
                    isOldPasswordValid = password == user.Password;
                }

                if (isPasswordValid || isOldPasswordValid)
                {
                    if (!user.IsActive)
                    {
                        ViewBag.ErrorMessage = "Ваш аккаунт не активен. Обратитесь к администратору.";
                        return View();
                    }

                    // Если вход по старому паролю — обновляем его в базе на хэш
                    if (isOldPasswordValid)
                    {
                        user.Password = PasswordHasher.HashPassword(password);
                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();
                    }

                    // Получаем все роли пользователя
                    var roles = _context.UserRoles
                        .Where(ur => ur.UserId == user.Id)
                        .Select(ur => ur.Role.Name)
                        .ToList();

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    TempData["UserName"] = user.Name;

                    return RedirectToAction("Dashboard");
                }
            }

            ViewBag.ErrorMessage = "Неверный логин или пароль";
            return View();
        }


        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: Профиль пользователя
        [HttpGet]
        public IActionResult Profile()
        {
            var userName = User.Identity.Name;
            var user = _context.Users
                .Include(u => u.UserRoles)  // Загружаем связанные роли
                .ThenInclude(ur => ur.Role)  // Загружаем роли через UserRole
                .FirstOrDefault(u => u.Login == userName);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);  // Передаем пользователя с ролями в представление
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Middlename = user.Middlename,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users.FindAsync(model.Id);
            if (user == null) return NotFound();

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Middlename = model.Middlename;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Profile));
        }

        // Метод для изменения пароля
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            var userLogin = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin);

            if (user != null && PasswordHasher.VerifyPassword(oldPassword, user.Password))
            {
                user.Password = PasswordHasher.HashPassword(newPassword);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Пароль успешно изменён!";
                return RedirectToAction(nameof(Profile));
            }

            ModelState.AddModelError("", "Неверный старый пароль");
            return View();
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }

}