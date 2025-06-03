using _1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using _1.Data;
using Microsoft.Extensions.Logging;

namespace _1.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IGDBContext _context;

        public ScheduleController(ILogger<ScheduleController> logger, IGDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index(int? eventTypeId, int? groupId)
        {
            var query = _context.Schedules
                .Include(s => s.ScheduleRoles)
                    .ThenInclude(sr => sr.Role)
                .Include(s => s.Event)
                .AsQueryable();

            if (eventTypeId.HasValue)
                query = query.Where(s => s.EventId == eventTypeId);

            if (groupId.HasValue)
                query = query.Where(s => s.ScheduleRoles.Any(sr => sr.RoleId == groupId));

            var schedules = await query
                .OrderBy(s => s.StartTime)
                .ToListAsync();

            ViewBag.Events = await _context.Events.ToListAsync();
            ViewBag.Groups = await _context.Roles.Where(r => r.Id >= 3 && r.Id <= 10).ToListAsync();

            return View(schedules);
        }

        [Authorize(Roles = "Администратор,Руководитель")]
        public IActionResult Create()
        {
            ViewData["Roles"] = _context.Roles.Where(r => r.Id >= 3 && r.Id <= 10).ToList(); 
            ViewData["Events"] = _context.Events.ToList(); 
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Администратор,Руководитель")]
        public async Task<IActionResult> Create(ScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var schedule = new Schedule
                {
                    EventId = model.EventId,
                    EventName = model.EventName,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    Location = model.Location,
                    AdditionalInfo = model.AdditionalInfo,
                    ScheduleRoles = model.RoleIds.Select(roleId => new ScheduleRole
                    {
                        RoleId = roleId
                    }).ToList()
                };

                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Roles"] = _context.Roles.Where(r => r.Id >= 3 && r.Id <= 10).ToList();
            ViewData["Events"] = _context.Events.ToList();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Администратор,Руководитель")]
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _context.Schedules
                .Include(s => s.ScheduleRoles)
                    .ThenInclude(sr => sr.Role)
                .Include(s => s.Event) 
                .FirstOrDefaultAsync(s => s.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор,Руководитель")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedules
                .Include(s => s.ScheduleRoles) 
                .FirstOrDefaultAsync(s => s.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }

            _context.ScheduleRoles.RemoveRange(schedule.ScheduleRoles);

            _context.Schedules.Remove(schedule);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var schedule = _context.Schedules
                .Include(s => s.ScheduleRoles)
                .ThenInclude(sr => sr.Role)
                .FirstOrDefault(s => s.Id == id);

            if (schedule == null)
                return NotFound();

            var viewModel = new ScheduleViewModel
            {
                Id = schedule.Id, 
                EventId = schedule.EventId,
                EventName = schedule.EventName,
                StartTime = schedule.StartTime.Value,
                EndTime = schedule.EndTime.Value,
                Location = schedule.Location,
                AdditionalInfo = schedule.AdditionalInfo,
                RoleIds = schedule.ScheduleRoles.Select(sr => sr.RoleId).ToList()
            };

            ViewBag.Roles = _context.Roles.Where(r => r.Id >= 3 && r.Id <= 10).ToList();
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Руководитель")]
        public async Task<IActionResult> Edit(ScheduleViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var schedule = _context.Schedules
                    .Include(s => s.ScheduleRoles)
                    .FirstOrDefault(s => s.Id == vm.Id);

                if (schedule == null)
                {
                    return NotFound();
                }

                schedule.StartTime = vm.StartTime;
                schedule.EndTime = vm.EndTime;
                schedule.Location = vm.Location;
                schedule.AdditionalInfo = vm.AdditionalInfo;

                if (schedule.ScheduleRoles == null)
                {
                    schedule.ScheduleRoles = new List<ScheduleRole>();
                }

                schedule.ScheduleRoles.Clear();

                foreach (var roleId in vm.RoleIds)
                {
                    var role = _context.Roles.FirstOrDefault(r => r.Id == roleId);
                    if (role != null)
                    {
                        schedule.ScheduleRoles.Add(new ScheduleRole
                        {
                            RoleId = role.Id,
                            ScheduleId = schedule.Id
                        });
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index)); 
            }

            ViewBag.Roles = _context.Roles.Where(r => r.Id >= 3 && r.Id <= 10).ToList(); 
            return View(vm);
        }
    }
}
