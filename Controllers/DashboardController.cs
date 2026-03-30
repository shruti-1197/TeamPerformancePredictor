using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TeamPerformancePredictor.Data;
using TeamPerformancePredictor.Services;

namespace TeamPerformancePredictor.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;
        private readonly MLPredictionService _ml;

        public DashboardController(AppDbContext db, MLPredictionService ml)
        {
            _db = db;
            _ml = ml;
        }

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
                var recent = emp.ActivityLogs
                    .OrderByDescending(l => l.WeekStartDate)
                    .Take(4).ToList();
                return _ml.Predict(emp.Id, recent);
            }).ToList();

            // Summary stats
            ViewBag.TotalEmployees   = employees.Count;
            ViewBag.TotalTeams       = await _db.Teams.CountAsync();
            ViewBag.HighRisk         = predictions.Count(p => p.BurnoutRiskLevel == "High");
            ViewBag.MediumRisk       = predictions.Count(p => p.BurnoutRiskLevel == "Medium");
            ViewBag.LowRisk          = predictions.Count(p => p.BurnoutRiskLevel == "Low");
            ViewBag.AvgProductivity  = predictions.Any() ? Math.Round(predictions.Average(p => p.ProductivityScore), 1) : 0;
            // Step 1: Zip with an explicit named selector → creates a clean anonymous type
            var paired = employees
                .Zip(predictions, (emp, pred) => new { Employee = emp, Prediction = pred })
                .ToList();

            // Step 2: Use OrderByDescending + FirstOrDefault instead of MaxBy
            // — more compatible across .NET versions and clearer to read
            ViewBag.TopPerformer = paired
                .OrderByDescending(x => x.Prediction.ProductivityScore)
                .FirstOrDefault()?.Employee.Name ?? "-";
            // Chart data (JSON for Chart.js)
            ViewBag.ChartLabels      = System.Text.Json.JsonSerializer.Serialize(employees.Select(e => e.Name));
            ViewBag.ChartScores      = System.Text.Json.JsonSerializer.Serialize(predictions.Select(p => p.ProductivityScore));
            ViewBag.ChartBurnout     = System.Text.Json.JsonSerializer.Serialize(predictions.Select(p => p.BurnoutProbability * 100));

            // Logged-in user info
            ViewBag.UserName         = User.FindFirst("FullName")?.Value ?? User.Identity?.Name;
            ViewBag.UserRole         = User.FindFirst(ClaimTypes.Role)?.Value;

            ViewBag.Employees        = employees;
            ViewBag.Predictions      = predictions;

            return View();
        }
    }
}
