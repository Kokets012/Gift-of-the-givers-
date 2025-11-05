using DAFWebApp.Data;
using DAFWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DAFWebApp.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly DAFDbContext _context;

        public VolunteerController(DAFDbContext context)
        {
            _context = context;
        }

        // Volunteer dashboard / list page
        public async Task<IActionResult> Index()
        {
            var volunteers = await _context.Volunteers
                                              .Include(v => v.VolunteerTasks)
                                              .ToListAsync();
            return View(volunteers);
        }

        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Volunteer volunteer)
        {
            if (!string.IsNullOrEmpty(volunteer.PasswordHash))
            {
                volunteer.PasswordHash = ComputeSha256Hash(volunteer.PasswordHash);
            }

            if (ModelState.IsValid)
            {
                _context.Volunteers.Add(volunteer);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Registered successfully!";
                return RedirectToAction("Login");
            }

            return View(volunteer);
        }

        // Volunteer login
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hash = ComputeSha256Hash(password);
            var volunteer = await _context.Volunteers.FirstOrDefaultAsync(v => v.Email == email && v.PasswordHash == hash);
            if (volunteer == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View();
            }

            HttpContext.Session.SetInt32("VolunteerId", volunteer.VolunteerId);
            HttpContext.Session.SetString("VolunteerName", volunteer.Name ?? "");

            return RedirectToAction("Index", "VolunteerTask");
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes) builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer != null)
            {
                _context.Volunteers.Remove(volunteer);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Volunteer deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: Volunteer/Edit/5
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null) return NotFound();

            return View(volunteer);
        }

        // POST: Volunteer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Volunteer volunteer)
        {
            if (id != volunteer.VolunteerId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(volunteer);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Volunteer updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(volunteer);
        }*/
    }
}
