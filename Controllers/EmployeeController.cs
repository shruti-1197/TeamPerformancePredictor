using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamPerformancePredictor.Data;
using TeamPerformancePredictor.Models;
using TeamPerformancePredictor.Services;

namespace TeamPerformancePredictor.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly MLPredictionService _ml;

        public EmployeeController(AppDbContext db, MLPredictionService ml)
        {
            _db = db;
            _ml = ml;
        }

        // GET /Employee
        public async Task<IActionResult> Index()
        {
            var employees = await _db.Employees
                .Include(e => e.Team)
                .Include(e => e.ActivityLogs)
                .Where(e => e.IsActive)
                .OrderBy(e => e.Name)
                .ToListAsync();

            var predictions = employees.Select(emp =>
            {
                var recent = emp.ActivityLogs.OrderByDescending(l => l.WeekStartDate).Take(4).ToList();
                return _ml.Predict(emp.Id, recent);
            }).ToList();

            ViewBag.Predictions = predictions;
            return View(employees);
        }

        // GET /Employee/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _db.Employees
                .Include(e => e.Team)
                .Include(e => e.ActivityLogs)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return NotFound();

            // Run prediction on all historical logs for trend analysis
            var allLogs = employee.ActivityLogs.OrderBy(l => l.WeekStartDate).ToList();
            var recentLogs = allLogs.TakeLast(4).ToList();
            var currentPrediction = _ml.Predict(id, recentLogs);

            // Build weekly trend data for charts
            var weeklyPredictions = new List<object>();
            for (int i = 0; i < allLogs.Count; i++)
            {
                var windowLogs = allLogs.Take(i + 1).TakeLast(4).ToList();
                var pred = _ml.Predict(id, windowLogs);
                weeklyPredictions.Add(new
                {
                    week = allLogs[i].WeekStartDate.ToString("MMM dd"),
                    productivity = pred.ProductivityScore,
                    burnoutProb = Math.Round(pred.BurnoutProbability * 100, 1),
                    hours = allLogs[i].HoursWorked,
                    tasks = allLogs[i].TasksCompleted
                });
            }

            ViewBag.CurrentPrediction  = currentPrediction;
            ViewBag.WeeklyTrend        = System.Text.Json.JsonSerializer.Serialize(weeklyPredictions);
            ViewBag.ActivityLogs       = allLogs;

            return View(employee);
        }

        // GET /Employee/Create
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Teams = new SelectList(await _db.Teams.ToListAsync(), "Id", "TeamName");
            return View();
        }

        // POST /Employee/Create
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Add(employee);
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Employee '{employee.Name}' added successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Teams = new SelectList(await _db.Teams.ToListAsync(), "Id", "TeamName");
            return View(employee);
        }

        // GET /Employee/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _db.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            ViewBag.Teams = new SelectList(await _db.Teams.ToListAsync(), "Id", "TeamName", employee.TeamId);
            return View(employee);
        }

        // POST /Employee/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                _db.Update(employee);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Employee updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Teams = new SelectList(await _db.Teams.ToListAsync(), "Id", "TeamName");
            return View(employee);
        }

      
        // POST /Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _db.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.IsActive = false; // Soft delete
                await _db.SaveChangesAsync();
                TempData["Success"] = "Employee removed successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
