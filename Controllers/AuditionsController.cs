using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _1.Models;
using _1.Data;

public class AuditionsController : Controller
{
    private readonly IGDBContext _context;

    public AuditionsController(IGDBContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var requests = await _context.AuditionRequests
            .Include(r => r.Audition)
            .ThenInclude(a => a.ResponsibleEmployee)
            .ToListAsync();

        var leaderRoleId = await _context.Roles
            .Where(r => r.Name == "Руководитель")
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        var leaders = await _context.Users
            .Where(u => u.UserRoles.Any(ur => ur.RoleId == leaderRoleId))
            .ToListAsync();

        var viewModel = requests.Select(r => new AuditionWithRequestViewModel
        {
            AuditionRequestId = r.Id,
            Name = r.Name,
            PhoneNumber = r.PhoneNumber,
            RequestDate = r.SubmittedAt,
            AuditionDate = r.Audition?.AuditionDate ?? DateTime.MinValue,
            ResponsibleEmployeeId = r.Audition?.ResponsibleEmployeeId,
            ResponsibleEmployeeName = r.Audition?.ResponsibleEmployee != null
                ? $"{r.Audition.ResponsibleEmployee.Surname} {r.Audition.ResponsibleEmployee.Name}"
                : null
        }).ToList();

        ViewBag.Leaders = leaders;

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AssignAudition(int auditionRequestId, DateTime auditionDate, int responsibleEmployeeId)
    {
        var request = await _context.AuditionRequests
            .Include(r => r.Audition)
            .FirstOrDefaultAsync(r => r.Id == auditionRequestId);

        if (request != null)
        {
            if (request.Audition == null)
            {
                request.Audition = new Audition
                {
                    AuditionDate = auditionDate,
                    AuditionRequestId = auditionRequestId,
                    ResponsibleEmployeeId = responsibleEmployeeId
                };
            }
            else
            {
                request.Audition.AuditionDate = auditionDate;
                request.Audition.ResponsibleEmployeeId = responsibleEmployeeId;
            }

            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelAudition([FromBody] CancelAuditionRequest data)
    {
        var request = await _context.AuditionRequests
            .Include(r => r.Audition)
            .FirstOrDefaultAsync(r => r.Id == data.AuditionRequestId);

        if (request?.Audition != null)
        {
            _context.Auditions.Remove(request.Audition);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        return BadRequest();
    }

    public class CancelAuditionRequest
    {
        public int AuditionRequestId { get; set; }
    }

}
