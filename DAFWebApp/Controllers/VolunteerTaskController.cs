using DAFWebApp.Data;
using DAFWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAFWebApp.Controllers
{
    public class VolunteerTaskController : Controller
    {
        private readonly DAFDbContext _context;

        public VolunteerTaskController(DAFDbContext context)
        {
            _context = context;
        }

        // List tasks
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.VolunteerTasks.Include(t => t.Volunteers).ToListAsync();
            return View(tasks);
        }

        // GET: Create task (admin)
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VolunteerTask task)
        {
            if (ModelState.IsValid)
            {
                _context.VolunteerTasks.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // Assign volunteer to task
        public async Task<IActionResult> Assign(int taskId)
        {
            var task = await _context.VolunteerTasks.Include(t => t.Volunteers).FirstOrDefaultAsync(t => t.TaskId == taskId);
            if (task == null) return NotFound();

            var volunteers = await _context.Volunteers.ToListAsync();
            ViewBag.Volunteers = volunteers;
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(int taskId, int volunteerId)
        {
            var task = await _context.VolunteerTasks.Include(t => t.Volunteers).FirstOrDefaultAsync(t => t.TaskId == taskId);
            var volunteer = await _context.Volunteers.FindAsync(volunteerId);

            if (task != null && volunteer != null)
            {
                task.Volunteers.Add(volunteer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
