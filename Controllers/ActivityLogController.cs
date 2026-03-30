using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamPerformancePredictor.Data;
using TeamPerformancePredictor.Models;

namespace TeamPerformancePredictor.Controllers
{
    [Authorize]
    public class ActivityLogController : Controller
    {
        private readonly AppDbContext _db;

        public ActivityLogController(AppDbContext db) => _db = db;

        // GET /ActivityLog
        public async Task<IActionResult> Index()
        {
            var logs = await _db.ActivityLogs
                .Include(l => l.Employee)
                    .ThenInclude(e => e!.Team)
                .OrderByDescending(l => l.WeekStartDate)
                .Take(100)
                .ToListAsync();
            return View(logs);
        }

        // GET /ActivityLog/Create
        public async Task<IActionResult> Create(int? employeeId)
        {
            ViewBag.Employees = new SelectList(
                await _db.Employees.Where(e => e.IsActive).OrderBy(e => e.Name).ToListAsync(),
                "Id", "Name", employeeId);
            return View(new ActivityLog { EmployeeId = employeeId ?? 0, WeekStartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek) });
        }

        // POST /ActivityLog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityLog log)
        {
            if (ModelState.IsValid)
            {
                log.RecordedAt = DateTime.Now;
                _db.ActivityLogs.Add(log);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Activity log saved. ML prediction will update on next dashboard load.";
                return RedirectToAction("Details", "Employee", new { id = log.EmployeeId });
            }
            ViewBag.Employees = new SelectList(await _db.Employees.Where(e => e.IsActive).ToListAsync(), "Id", "Name");
            return View(log);
        }
    }
}
