using DAFWebApp.Data;
using DAFWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAFWebApp.Controllers
{
    public class DonationsController : Controller
    {
        private readonly DAFDbContext _context;

        public DonationsController(DAFDbContext context)
        {
            _context = context;
        }

        // List all donations
        public async Task<IActionResult> Index()
        {
            var donations = await _context.Donations.ToListAsync();
            return View(donations);
        }

        // GET: Donation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Donation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Donation donation)
        {
            if (ModelState.IsValid)
            {
                _context.Donations.Add(donation);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Donation submitted successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(donation);
        }

        // Optional: Delete donation
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null) return NotFound();

            return View(donation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation != null)
            {
                _context.Donations.Remove(donation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
