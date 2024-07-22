using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PasswordManager.Controllers
{
    public class WeblistController : Controller
    {
        private readonly ILogger<WeblistController> _logger;
        private readonly ApplicationDbContext _db;

        public WeblistController(ILogger<WeblistController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Add(Guid id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.UserId = user.Id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(WebList web)
        {
                web.Id = Guid.NewGuid();
                await _db.WebLists.AddAsync(web);
                await _db.SaveChangesAsync();
                return RedirectToAction("Detail", "Home", new { id = web.UserId });

        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var web = await _db.WebLists.FirstOrDefaultAsync(x => x.Id == id);
            return View(web);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id, Name, Title,Password,UserId")] WebList web)
        {
            var webToUpdate = _db.WebLists.FirstOrDefault(x => x.Id == web.Id);
            if (webToUpdate == null)
            {
                return NotFound();
            }
            webToUpdate.Id = web.Id;
            webToUpdate.Name = web.Name;
            webToUpdate.Title = web.Title;
            webToUpdate.Password = web.Password;

            _db.WebLists.Update(webToUpdate);
            await _db.SaveChangesAsync();
            return RedirectToAction("Detail", "Home", new { id = web.UserId });

        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var web = await _db.WebLists.FirstOrDefaultAsync(x=>x.Id == id);
            if (web == null)
            {
                return NotFound();
            }
            return View(web);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(WebList web)
        {
            var userId = web.UserId;
            _db.WebLists.Remove(web);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Detail(Guid id)
        {
            var web = await _db.WebLists.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == id);
            if (web == null)
            {
                return NotFound();
            }

            return View(web);
        }
    }
}
