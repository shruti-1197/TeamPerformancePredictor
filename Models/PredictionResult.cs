using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamPerformancePredictor.Models
{
    public class PredictionResult
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DateTime PredictionDate { get; set; } = DateTime.Now;

        // ML Regression output: 0-100
        [Range(0, 100)]
        public float ProductivityScore { get; set; }

        // ML Classification output: "Low", "Medium", "High"
        public string BurnoutRiskLevel { get; set; } = "Low";

        // Numeric burnout probability (0.0 - 1.0)
        public float BurnoutProbability { get; set; }

        // Action recommendation text
        public string Recommendation { get; set; } = string.Empty;

        // Which ML model was used
        public string ModelUsed { get; set; } = "RuleEngine";

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }
    }
}
