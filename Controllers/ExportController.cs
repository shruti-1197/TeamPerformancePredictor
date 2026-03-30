using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamPerformancePredictor.Data;
using TeamPerformancePredictor.Services;

namespace TeamPerformancePredictor.Controllers
{
    [Authorize]
    public class ExportController : Controller
    {
        private readonly AppDbContext _db;
        private readonly MLPredictionService _ml;

        public ExportController(AppDbContext db, MLPredictionService ml)
        {
            _db = db;
            _ml = ml;
        }

        /// <summary>
        /// Exports a full ML predictions report as an Excel file.
        /// GET /Export/Predictions
        /// </summary>
        public async Task<IActionResult> Predictions()
        {
            var employees = await _db.Employees
                .Include(e => e.Team)
                .Include(e => e.ActivityLogs)
                .Where(e => e.IsActive)
                .OrderBy(e => e.Name)
                .ToListAsync();

            using var workbook = new XLWorkbook();

            // ── Sheet 1: Predictions Summary ──
            var ws1 = workbook.Worksheets.Add("ML Predictions");

            // Title row
            ws1.Cell(1, 1).Value = "Team Performance Predictor — ML Predictions Report";
            ws1.Range(1, 1, 1, 8).Merge().Style
                .Font.SetBold(true).Font.SetFontSize(14)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#0B1437"))
                .Font.SetFontColor(XLColor.White)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws1.Cell(2, 1).Value = $"Generated: {DateTime.Now:dd MMM yyyy HH:mm}";
            ws1.Range(2, 1, 2, 8).Merge().Style
                .Font.SetFontColor(XLColor.FromHtml("#888888"))
                .Font.SetItalic(true)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Headers row
            var headers = new[] { "Employee", "Email", "Team", "Designation", "Productivity Score", "Burnout Risk", "Burnout Probability", "ML Recommendation" };
            for (int c = 0; c < headers.Length; c++)
            {
                ws1.Cell(4, c + 1).Value = headers[c];
            }
            ws1.Range(4, 1, 4, 8).Style
                .Font.SetBold(true).Font.SetFontSize(11)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#00C9A7"))
                .Font.SetFontColor(XLColor.White)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Data rows
            int row = 5;
            foreach (var emp in employees)
            {
                var recent = emp.ActivityLogs.OrderByDescending(l => l.WeekStartDate).Take(4).ToList();
                var pred = _ml.Predict(emp.Id, recent);

                ws1.Cell(row, 1).Value = emp.Name;
                ws1.Cell(row, 2).Value = emp.Email;
                ws1.Cell(row, 3).Value = emp.Team?.TeamName ?? "-";
                ws1.Cell(row, 4).Value = emp.Designation;
                ws1.Cell(row, 5).Value = pred.ProductivityScore;
                ws1.Cell(row, 6).Value = pred.BurnoutRiskLevel;
                ws1.Cell(row, 7).Value = $"{Math.Round(pred.BurnoutProbability * 100, 0)}%";
                ws1.Cell(row, 8).Value = pred.Recommendation;

                // Color-code burnout risk cell
                var riskCell = ws1.Cell(row, 6);
                riskCell.Style.Font.SetBold(true);
                riskCell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                if (pred.BurnoutRiskLevel == "High")
                    riskCell.Style.Font.SetFontColor(XLColor.FromHtml("#FF5C6C"));
                else if (pred.BurnoutRiskLevel == "Medium")
                    riskCell.Style.Font.SetFontColor(XLColor.FromHtml("#d97706"));
                else
                    riskCell.Style.Font.SetFontColor(XLColor.FromHtml("#00a589"));

                // Alternate row shading
                if (row % 2 == 0)
                    ws1.Range(row, 1, row, 8).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#F4F7FE"));

                row++;
            }

            // Auto-fit columns
            ws1.Columns().AdjustToContents();
            ws1.Column(8).Width = 60; // Recommendation column — wider

            // ── Sheet 2: Raw Activity Logs ──
            var ws2 = workbook.Worksheets.Add("Activity Logs");
            var logHeaders = new[] { "Employee", "Team", "Week", "Hours Worked", "Tasks", "Meetings", "Overtime (hrs)", "Leave Days", "Deadlines Missed", "Peer Score" };
            for (int c = 0; c < logHeaders.Length; c++)
                ws2.Cell(1, c + 1).Value = logHeaders[c];

            ws2.Range(1, 1, 1, 10).Style
                .Font.SetBold(true)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#0B1437"))
                .Font.SetFontColor(XLColor.White);

            int logRow = 2;
            var allLogs = await _db.ActivityLogs
                .Include(l => l.Employee).ThenInclude(e => e!.Team)
                .OrderByDescending(l => l.WeekStartDate)
                .ToListAsync();

            foreach (var log in allLogs)
            {
                ws2.Cell(logRow, 1).Value = log.Employee?.Name;
                ws2.Cell(logRow, 2).Value = log.Employee?.Team?.TeamName;
                ws2.Cell(logRow, 3).Value = log.WeekStartDate.ToString("dd MMM yyyy");
                ws2.Cell(logRow, 4).Value = log.HoursWorked;
                ws2.Cell(logRow, 5).Value = log.TasksCompleted;
                ws2.Cell(logRow, 6).Value = log.MeetingsAttended;
                ws2.Cell(logRow, 7).Value = log.OvertimeHours;
                ws2.Cell(logRow, 8).Value = log.LeaveDaysTaken;
                ws2.Cell(logRow, 9).Value = log.DeadlinesMissed;
                ws2.Cell(logRow, 10).Value = log.PeerCollaborationScore;

                if (logRow % 2 == 0)
                    ws2.Range(logRow, 1, logRow, 10).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#F4F7FE"));
                logRow++;
            }
            ws2.Columns().AdjustToContents();

            // Stream as download
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            var fileName = $"TPP_Report_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}
