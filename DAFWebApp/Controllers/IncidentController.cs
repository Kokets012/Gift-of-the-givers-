using DAFWebApp.Data;
using DAFWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


namespace DAFWebApp.Controllers
{
    public class IncidentController : Controller
    {
        private readonly DAFDbContext _context;

        public IncidentController(DAFDbContext context)
        {
            _context = context;
        }

        // Show all incidents
        public async Task<IActionResult> Index()
        {
            var incidents = await _context.Incidents
                .Include(i => i.User)
                .ToListAsync();
            return View(incidents);
        }

        // GET: Report Incident form
        public IActionResult Create()
        {
            return View();
        }

        // POST: Save Incident
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Incident incident)
        {
            if (ModelState.IsValid)
            {
                // Attach logged-in user
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    ModelState.AddModelError("", "You must be logged in to report an incident.");
                    return View(incident);
                }

                incident.UserId = userId.Value;
                incident.DateReported = DateTime.Now;

                _context.Incidents.Add(incident);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Incident reported successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(incident);
        }

        // Details page
        public async Task<IActionResult> Details(int id)
        {
            var incident = await _context.Incidents
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.IncidentId == id);

            if (incident == null) return NotFound();

            return View(incident);
        }

        // GET: /Incident/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }
            return View(incident);
        }

        // POST: /Incident/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Incident incident)
        {
            if (id != incident.IncidentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incident);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return BadRequest("Error updating incident");
                }
            }
            return View(incident);
        }

        // GET: /Incident/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .FirstOrDefaultAsync(m => m.IncidentId == id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // POST: /Incident/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident != null)
            {
                _context.Incidents.Remove(incident);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
