using System.Diagnostics;
using _1.Models;
using _1.Data;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;

namespace _1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IGDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Dashboard", "Account");

            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private readonly IGDBContext _context;

        [HttpPost]
        public async Task<IActionResult> SubmitRequest(string name, string phoneNumber)
        {
            // �������� ������� ������
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                return BadRequest(new { message = "��� � ����� �������� ����������� ��� ����������." });
            }

            // ����� �������� �������������� ��������� ������ �������� (��������, �������� �������)
            if (!System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{11}$"))
            {
                return BadRequest(new { message = "����� �������� ������ ��������� ����� 11 ����." });
            }

            // �������� � ���������� ����� ������
            var request = new AuditionRequest
            {
                Name = name,
                PhoneNumber = phoneNumber,
                SubmittedAt = DateTime.Now
            };

            _context.AuditionRequests.Add(request);
            await _context.SaveChangesAsync();

            // ������� JSON-������ �� ������
            return Ok(new { message = "���� ������ ������� ����������!" });
        }

    }
}
