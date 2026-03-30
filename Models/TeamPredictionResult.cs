using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamPerformancePredictor.Models
{
    public class TeamPredictionResult
    {
        public int Id { get; set; }
        public int TeamId { get; set; }

        public float AvgProductivityScore { get; set; }
        public float AvgBurnoutProbability { get; set; }
        public string BurnoutRiskLevel { get; set; } = "Low";
        public string PerformanceGrade { get; set; } = "B";

        public float AvgHoursWorked { get; set; }
        public float AvgTasksCompleted { get; set; }
        public float AvgCollaborationScore { get; set; }
        public float AvgCodeCommits { get; set; }
        public int HighRiskEmployeeCount { get; set; }
        public int TotalEmployees { get; set; }

        public string TopPerformerName { get; set; } = "";
        public float TopPerformerScore { get; set; }

        public DateTime PredictedAt { get; set; } = DateTime.Now;

        [ForeignKey("TeamId")]
        public Team? Team { get; set; }
    }
}