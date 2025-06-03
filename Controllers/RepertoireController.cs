using _1.Models;
using _1.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace _1.Controllers
{
    public class RepertoireController : Controller
    {
        private readonly RepertoireService _service;

        public RepertoireController(RepertoireService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var entries = _service.GetAll();
            return View(entries);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор,Руководитель")]
        public IActionResult Add(string htmlContent)
        {
            _service.Add(new RepertoireEntry { HtmlContent = htmlContent });
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Администратор,Руководитель")]
        public IActionResult Edit(Guid id, string htmlContent)
        {
            _service.Update(id, htmlContent);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Администратор,Руководитель")]
        public IActionResult Delete(Guid id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
