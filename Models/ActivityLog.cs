using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamPerformancePredictor.Models
{
    /// <summary>
    /// Stores weekly work activity data for each employee.
    /// These features are used as ML model inputs.
    /// </summary>
    public class ActivityLog
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        public DateTime WeekStartDate { get; set; }

        // ---- ML Feature 1: Work Hours ----
        [Range(0, 168, ErrorMessage = "Hours must be 0-168")]
        [Display(Name = "Hours Worked")]
        public float HoursWorked { get; set; }

        // ---- ML Feature 2: Output / Deliverables ----
        [Range(0, 100)]
        [Display(Name = "Tasks Completed")]
        public float TasksCompleted { get; set; }

        // ---- ML Feature 3: Collaboration ----
        [Range(0, 50)]
        [Display(Name = "Meetings Attended")]
        public float MeetingsAttended { get; set; }

        // ---- ML Feature 4: Overwork Signal ----
        [Range(0, 80)]
        [Display(Name = "Overtime Hours")]
        public float OvertimeHours { get; set; }

        // ---- ML Feature 5: Absence Signal ----
        [Range(0, 7)]
        [Display(Name = "Leave Days Taken")]
        public float LeaveDaysTaken { get; set; }

        // ---- ML Feature 6: Deadline Pressure ----
        [Range(0, 20)]
        [Display(Name = "Deadlines Missed")]
        public float DeadlinesMissed { get; set; }

        // ---- ML Feature 7: Peer rating (1-5 scale) ----
        [Range(1, 5)]
        [Display(Name = "Peer Collaboration Score")]
        public float PeerCollaborationScore { get; set; } = 3;

        // ---- ML Feature 8: Code Commits / Deliverables ----
        [Range(0, 500)]
        [Display(Name = "Code Commits")]
        public float CodeCommits { get; set; } = 0;

        public DateTime RecordedAt { get; set; } = DateTime.Now;

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }
    }
}
