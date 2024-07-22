using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PasswordManager.Models;

namespace PasswordManager.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AdminUser> _userManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<AdminUser> userManager)
    {
        _logger = logger;
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var adminUser = _userManager.GetUserAsync(User).Result;
        if (adminUser == null){
            return NotFound();
        }
        var user = await _db.Users.Include(x => x.WebLists).Where(x => x.AdminUserId == adminUser.Id).ToListAsync();
        return View(user);
    }

    public IActionResult Create()
    {
        return View();
    }

[HttpPost]
    public async Task<IActionResult> Create(AppUser user, IFormFile imageFile)
    {
        var admin = await _userManager.GetUserAsync(User);
            if (admin == null)
            {
                return RedirectToAction("Login", "Account");
            }
            user.AdminUserId = admin.Id;
        if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "userImage");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                user.ImagePath = "/userImage/" + uniqueFileName;
            }
            else
            {
                user.ImagePath = "/userImage/default.jpg"; // Set a default image path if no image is provided
            }
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
     public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,[Bind("Id,Name,ImagePath")] AppUser user, IFormFile imageFile)
        {
            var userToUpdate = _db.Users.FirstOrDefault(s => s.Id == user.Id);

            if (userToUpdate == null)
            {
                return NotFound();
            }
            userToUpdate.Id = user.Id;
            userToUpdate.Name = user.Name;
            
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "userImage");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                userToUpdate.ImagePath = "/userImage/" + uniqueFileName;
            }

            _db.Users.Update(userToUpdate);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Delete(Guid id)
        {

            var userToDelete = _db.Users.FirstOrDefault(x => x.Id == id);

            if (userToDelete == null)
            {
                return NotFound();
            }

            return View(userToDelete);

        }

        [HttpPost]
        public IActionResult Delete(AppUser user)
        {
            _db.Users.Remove(user);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");

        }
    public async Task<IActionResult> Detail(Guid id)
    {
        var user = await _db.Users.Include(x=>x.WebLists).FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
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
}
