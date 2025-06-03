using Microsoft.AspNetCore.Mvc;
using _1.Models; 
using _1.Data;  
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using _1.Utils;

namespace _1.Controllers
{
    public class UserController : Controller
    {
        private readonly IGDBContext _context;

        public UserController(IGDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new User()); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.IsActive = true;
                user.Password = PasswordHasher.HashPassword(user.Password); 

                _context.Users.Add(user);
                _context.SaveChanges();

                if (user.Id == 0)
                {
                    throw new Exception("Ошибка: user.Id не сгенерирован после сохранения!");
                }

                return RedirectToAction("Index");
            }
            return View(user);
        }
        public async Task<IActionResult> Index(string search, string role)
        {
            var usersQuery = _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                usersQuery = usersQuery.Where(u =>
                    u.Name.Contains(search) || u.Surname.Contains(search));
            }

            if (!string.IsNullOrEmpty(role))
            {
                usersQuery = usersQuery.Where(u =>
                    u.UserRoles.Any(ur => ur.Role.Name == role));
            }

            var users = await usersQuery.ToListAsync();

            ViewBag.Roles = await _context.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.SelectedRole = role;
            ViewBag.SearchQuery = search;

            return View(users);
        }

        [HttpPost]
        //[Authorize(Roles = "Администратор")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AssignRoles(int id)
        {
            var user = _context.Users
                               .Include(u => u.UserRoles)
                               .ThenInclude(ur => ur.Role)
                               .FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound();

            ViewBag.AllRoles = _context.Roles.ToList();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Администратор")]
        public IActionResult AssignRoles(int id, int[] selectedRoles)
        {
            var user = _context.Users
                              .Include(u => u.UserRoles)
                              .ThenInclude(ur => ur.Role)
                              .FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound();

            var currentRoles = user.UserRoles.Select(ur => ur.RoleId).ToArray();
            var rolesToRemove = currentRoles.Except(selectedRoles).ToArray();
            var rolesToAdd = selectedRoles.Except(currentRoles).ToArray();

            foreach (var roleId in rolesToRemove)
            {
                var userRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == id && ur.RoleId == roleId);
                if (userRole != null)
                {
                    _context.UserRoles.Remove(userRole);
                }
            }

            foreach (var roleId in rolesToAdd)
            {
                var role = _context.Roles.Find(roleId);
                if (role == null)
                {
                    continue; 
                }

                _context.UserRoles.Add(new UserRole
                {
                    UserId = id,
                    RoleId = roleId
                });
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ModelState.AddModelError("", $"Конкурентное обновление произошло: {ex.Message}");
                return View(user);
            }

            return RedirectToAction("Index");
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

            return RedirectToAction(nameof(Index));
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
