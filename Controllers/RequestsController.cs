using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _1.Models;
using _1.Data;

public class RequestsController : Controller
{
    private readonly IGDBContext _context;

    public RequestsController(IGDBContext context)
    {
        _context = context;
    }

    // GET: /Requests
    [Authorize(Roles = "Администратор,Руководитель")]
    public async Task<IActionResult> Index()
    {
        var requests = await _context.AuditionRequests
            .Include(r => r.Audition)
            .OrderByDescending(r => r.SubmittedAt)
            .ToListAsync();
        return View(requests);
    }

}
