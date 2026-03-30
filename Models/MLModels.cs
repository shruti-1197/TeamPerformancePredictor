using Microsoft.ML.Data;

namespace TeamPerformancePredictor.Models
{
    /// <summary>
    /// Input schema for ML.NET — matches the features in ActivityLog.
    /// ML.NET requires [LoadColumn] attributes to map CSV columns.
    /// </summary>
    public class EmployeeMLInput
    {
        [LoadColumn(0)] public float HoursWorked { get; set; }
        [LoadColumn(1)] public float TasksCompleted { get; set; }
        [LoadColumn(2)] public float MeetingsAttended { get; set; }
        [LoadColumn(3)] public float OvertimeHours { get; set; }
        [LoadColumn(4)] public float LeaveDaysTaken { get; set; }
        [LoadColumn(5)] public float DeadlinesMissed { get; set; }
        [LoadColumn(6)] public float PeerCollaborationScore { get; set; }

        // For regression: the score we want to predict (Label)
        [LoadColumn(7)] public float ProductivityScore { get; set; }

        // For classification: burnout label we want to predict
        [LoadColumn(8)] public string BurnoutLabel { get; set; } = "Low";
    }

    /// <summary>
    /// Output schema for regression (productivity score prediction)
    /// </summary>
    public class ProductivityPrediction
    {
        [ColumnName("Score")]
        public float PredictedScore { get; set; }
    }

    /// <summary>
    /// Output schema for classification (burnout risk prediction)
    /// </summary>
    public class BurnoutPrediction
    {
        [ColumnName("PredictedLabel")]
        public string? PredictedLabel { get; set; }

        [ColumnName("Score")]
        public float[]? Score { get; set; }
    }
}
