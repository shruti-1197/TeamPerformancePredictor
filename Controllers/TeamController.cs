using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamPerformancePredictor.Data;
using TeamPerformancePredictor.Models;

namespace TeamPerformancePredictor.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _db;

        public TeamController(AppDbContext db)
        {
            _db = db;
        }

        // GET /Team
        public async Task<IActionResult> Index()
        {
            var teams = await _db.Teams
                .Include(t => t.Employees)
                .ToListAsync();
            return View(teams);
        }

        // GET /Team/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST /Team/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Team team)
        {
            if (ModelState.IsValid)
            {
                _db.Teams.Add(team);
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Team '{team.TeamName}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // POST /Team/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _db.Teams.FindAsync(id);
            if (team != null)
            {
                _db.Teams.Remove(team);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Team deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}