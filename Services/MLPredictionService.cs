using Microsoft.ML;
using Microsoft.ML.Data;
using TeamPerformancePredictor.Models;

namespace TeamPerformancePredictor.Services
{
    /// <summary>
    /// ML-powered prediction engine using Microsoft ML.NET.
    ///
    /// Architecture:
    ///   1. Regression model (FastTree) → predicts ProductivityScore (0–100)
    ///   2. Multi-class classification (SdcaMaximumEntropy) → predicts BurnoutRisk (Low/Medium/High)
    ///
    /// Training: Uses synthetic + seed data to train both models at startup.
    /// Inference: Runs both models on new employee activity data.
    /// </summary>
    public class MLPredictionService
    {
        private readonly MLContext _mlContext;

        // Trained model pipelines
        private ITransformer? _productivityModel;
        private ITransformer? _burnoutModel;

        // Prediction engines (reusable, thread-safe wrappers)
        private PredictionEngine<EmployeeMLInput, ProductivityPrediction>? _productivityEngine;
        private PredictionEngine<EmployeeMLInput, BurnoutPrediction>? _burnoutEngine;

        private bool _isTrained = false;
        private readonly ILogger<MLPredictionService> _logger;

        public MLPredictionService(ILogger<MLPredictionService> logger)
        {
            _logger = logger;
            _mlContext = new MLContext(seed: 42); // seed=42 for reproducibility
        }

        /// <summary>
        /// Trains both ML models using the provided labeled data.
        /// Call this once at startup with historical activity logs.
        /// </summary>
        public void TrainModels(List<EmployeeMLInput> trainingData)
        {
            if (trainingData.Count < 10)
            {
                _logger.LogWarning("Insufficient training data ({Count} rows). Using rule-based fallback.", trainingData.Count);
                return;
            }

            try
            {
                _logger.LogInformation("Training ML models with {Count} records...", trainingData.Count);

                var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

                // ---- Train Productivity Regression Model ----
                // Pipeline: normalize features → FastTree regression
                var regressionPipeline = _mlContext.Transforms
                    .Concatenate("Features",
                        nameof(EmployeeMLInput.HoursWorked),
                        nameof(EmployeeMLInput.TasksCompleted),
                        nameof(EmployeeMLInput.MeetingsAttended),
                        nameof(EmployeeMLInput.OvertimeHours),
                        nameof(EmployeeMLInput.LeaveDaysTaken),
                        nameof(EmployeeMLInput.DeadlinesMissed),
                        nameof(EmployeeMLInput.PeerCollaborationScore))
                    .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                    .Append(_mlContext.Transforms.CopyColumns("Label", nameof(EmployeeMLInput.ProductivityScore)))
                    .Append(_mlContext.Regression.Trainers.FastTree(
                        labelColumnName: "Label",
                        featureColumnName: "Features",
                        numberOfLeaves: 20,
                        numberOfTrees: 100,
                        minimumExampleCountPerLeaf: 2));

                _productivityModel = regressionPipeline.Fit(dataView);

                // ---- Train Burnout Classification Model ----
                // Pipeline: normalize features → SDCA multi-class classification
                var classificationPipeline = _mlContext.Transforms
                    .Concatenate("Features",
                        nameof(EmployeeMLInput.HoursWorked),
                        nameof(EmployeeMLInput.TasksCompleted),
                        nameof(EmployeeMLInput.MeetingsAttended),
                        nameof(EmployeeMLInput.OvertimeHours),
                        nameof(EmployeeMLInput.LeaveDaysTaken),
                        nameof(EmployeeMLInput.DeadlinesMissed),
                        nameof(EmployeeMLInput.PeerCollaborationScore))
                    .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                    .Append(_mlContext.Transforms.Conversion.MapValueToKey(
                        inputColumnName: nameof(EmployeeMLInput.BurnoutLabel),
                        outputColumnName: "Label"))
                    .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(
                        labelColumnName: "Label",
                        featureColumnName: "Features",
                        maximumNumberOfIterations: 100))
                    .Append(_mlContext.Transforms.Conversion.MapKeyToValue(
                        inputColumnName: "PredictedLabel",
                        outputColumnName: "PredictedLabel"));

                _burnoutModel = classificationPipeline.Fit(dataView);

                // ---- Create reusable prediction engines ----
                _productivityEngine = _mlContext.Model.CreatePredictionEngine<EmployeeMLInput, ProductivityPrediction>(_productivityModel);
                _burnoutEngine = _mlContext.Model.CreatePredictionEngine<EmployeeMLInput, BurnoutPrediction>(_burnoutModel);

                _isTrained = true;
                _logger.LogInformation("ML models trained successfully.");

                // ---- Evaluate regression model ----
                var predictions = _productivityModel.Transform(dataView);
                var metrics = _mlContext.Regression.Evaluate(predictions, "Label", "Score");
                _logger.LogInformation("Regression R²: {R2:F3}, MAE: {MAE:F2}", metrics.RSquared, metrics.MeanAbsoluteError);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ML model training failed. Using rule-based fallback.");
                _isTrained = false;
            }
        }

        /// <summary>
        /// Runs prediction on an employee's recent activity logs.
        /// Uses ML models if trained, otherwise falls back to rule-based logic.
        /// </summary>
        public PredictionResult Predict(int employeeId, List<ActivityLog> recentLogs)
        {
            if (!recentLogs.Any())
            {
                return new PredictionResult
                {
                    EmployeeId = employeeId,
                    ProductivityScore = 0,
                    BurnoutRiskLevel = "Unknown",
                    BurnoutProbability = 0,
                    Recommendation = "No activity data available. Please log weekly activity data to generate predictions.",
                    ModelUsed = "NoData"
                };
            }

            // Aggregate last N weeks into a single feature vector (average)
            var input = new EmployeeMLInput
            {
                HoursWorked = recentLogs.Average(l => l.HoursWorked),
                TasksCompleted = recentLogs.Average(l => l.TasksCompleted),
                MeetingsAttended = recentLogs.Average(l => l.MeetingsAttended),
                OvertimeHours = recentLogs.Average(l => l.OvertimeHours),
                LeaveDaysTaken = recentLogs.Average(l => l.LeaveDaysTaken),
                DeadlinesMissed = recentLogs.Average(l => l.DeadlinesMissed),
                PeerCollaborationScore = recentLogs.Average(l => l.PeerCollaborationScore)
            };

            if (_isTrained && _productivityEngine != null && _burnoutEngine != null)
            {
                return PredictWithML(employeeId, input);
            }
            else
            {
                return PredictWithRules(employeeId, input);
            }
        }

        // ---- ML-based prediction ----
        private PredictionResult PredictWithML(int employeeId, EmployeeMLInput input)
        {
            var prodPred = _productivityEngine!.Predict(input);
            var burnPred = _burnoutEngine!.Predict(input);

            float score = Math.Clamp(prodPred.PredictedScore, 0f, 100f);
            string burnoutLevel = burnPred.PredictedLabel ?? "Low";

            // Estimate burnout probability from score array
            float burnoutProb = 0f;
            if (burnPred.Score != null && burnPred.Score.Length >= 3)
            {
                // Score[0]=High, Score[1]=Low, Score[2]=Medium (alphabetical key encoding)
                burnoutProb = burnPred.Score.Max();
            }

            return new PredictionResult
            {
                EmployeeId = employeeId,
                ProductivityScore = (float)Math.Round(score, 1),
                BurnoutRiskLevel = burnoutLevel,
                BurnoutProbability = (float)Math.Round(burnoutProb, 2),
                Recommendation = GenerateRecommendation(burnoutLevel, score, input),
                ModelUsed = "ML.NET (FastTree + SDCA)"
            };
        }

        // ---- Rule-based fallback ----
        private PredictionResult PredictWithRules(int employeeId, EmployeeMLInput input)
        {
            // Weighted productivity formula
            float taskScore      = Math.Min(input.TasksCompleted / 12f, 1f) * 35f;
            float hoursScore     = input.HoursWorked switch {
                >= 35 and <= 44 => 25f,
                >= 25 and < 35  => (input.HoursWorked - 25f) / 10f * 25f,
                > 44            => Math.Max(0, 25f - (input.HoursWorked - 44f) * 1.2f),
                _               => 0f
            };
            float peerScore      = (input.PeerCollaborationScore / 5f) * 20f;
            float deadlineScore  = Math.Max(0, (5f - input.DeadlinesMissed) / 5f) * 10f;
            float availScore     = Math.Max(0, (5f - input.LeaveDaysTaken) / 5f) * 10f;

            float score = Math.Clamp(taskScore + hoursScore + peerScore + deadlineScore + availScore, 0f, 100f);

            string burnout;
            float prob;
            if (input.OvertimeHours >= 18 && input.LeaveDaysTaken == 0 && input.DeadlinesMissed >= 2)
            { burnout = "High";   prob = 0.85f; }
            else if (input.OvertimeHours >= 10 || (input.HoursWorked > 50))
            { burnout = "Medium"; prob = 0.55f; }
            else
            { burnout = "Low";    prob = 0.15f; }

            return new PredictionResult
            {
                EmployeeId = employeeId,
                ProductivityScore = (float)Math.Round(score, 1),
                BurnoutRiskLevel = burnout,
                BurnoutProbability = prob,
                Recommendation = GenerateRecommendation(burnout, score, input),
                ModelUsed = "Rule Engine"
            };
        }

        // ---- Smart recommendation generator ----
        private static string GenerateRecommendation(string burnout, float score, EmployeeMLInput input)
        {
            var tips = new List<string>();

            if (burnout == "High")
                tips.Add("🚨 URGENT: Immediate workload intervention required. Schedule 1-on-1 meeting this week.");
            else if (burnout == "Medium")
                tips.Add("⚠️ Elevated burnout risk detected. Review workload distribution.");

            if (input.OvertimeHours > 15)
                tips.Add($"Overtime averaging {input.OvertimeHours:F0}h/week — enforce work-hour limits.");
            if (input.LeaveDaysTaken == 0 && input.OvertimeHours > 8)
                tips.Add("No breaks taken recently — encourage time off to prevent burnout.");
            if (input.DeadlinesMissed > 1)
                tips.Add($"{input.DeadlinesMissed:F0} deadlines missed — identify blockers and provide support.");
            if (input.TasksCompleted < 4)
                tips.Add("Low task output — check for resource gaps or unclear priorities.");
            if (input.PeerCollaborationScore < 3)
                tips.Add("Low collaboration score — consider team-building activities.");

            if (score >= 80 && burnout == "Low")
                tips.Add("✅ Excellent performance. Consider this employee for recognition or leadership opportunities.");
            else if (score >= 60)
                tips.Add("👍 Healthy performance level. Continue regular check-ins.");

            return tips.Any() ? string.Join(" | ", tips) : "Performance is within normal range.";
        }

        /// <summary>
        /// Generates labeled training data from historical activity logs.
        /// Labels are computed using business rules to bootstrap the ML model.
        /// </summary>
        public static List<EmployeeMLInput> GenerateTrainingData(List<ActivityLog> logs)
        {
            return logs.Select(log =>
            {
                // Compute ground-truth productivity score using weighted formula
                float taskScore     = Math.Min(log.TasksCompleted / 12f, 1f) * 35f;
                float hoursScore    = log.HoursWorked switch {
                    >= 35 and <= 44 => 25f,
                    >= 25 and < 35  => (log.HoursWorked - 25f) / 10f * 25f,
                    > 44            => Math.Max(0, 25f - (log.HoursWorked - 44f) * 1.2f),
                    _               => 0f
                };
                float peerScore     = (log.PeerCollaborationScore / 5f) * 20f;
                float deadlineScore = Math.Max(0, (5f - log.DeadlinesMissed) / 5f) * 10f;
                float availScore    = Math.Max(0, (5f - log.LeaveDaysTaken) / 5f) * 10f;
                float prodScore     = Math.Clamp(taskScore + hoursScore + peerScore + deadlineScore + availScore, 0f, 100f);

                // Compute ground-truth burnout label
                string burnout;
                if (log.OvertimeHours >= 18 && log.LeaveDaysTaken == 0)
                    burnout = "High";
                else if (log.OvertimeHours >= 10 || log.HoursWorked > 50)
                    burnout = "Medium";
                else
                    burnout = "Low";

                return new EmployeeMLInput
                {
                    HoursWorked = log.HoursWorked,
                    TasksCompleted = log.TasksCompleted,
                    MeetingsAttended = log.MeetingsAttended,
                    OvertimeHours = log.OvertimeHours,
                    LeaveDaysTaken = log.LeaveDaysTaken,
                    DeadlinesMissed = log.DeadlinesMissed,
                    PeerCollaborationScore = log.PeerCollaborationScore,
                    ProductivityScore = prodScore,
                    BurnoutLabel = burnout
                };
            }).ToList();
        }
    }
}
