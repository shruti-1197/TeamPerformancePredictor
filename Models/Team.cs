using System.ComponentModel.DataAnnotations;

namespace TeamPerformancePredictor.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TeamName { get; set; } = string.Empty;

        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        public string? ManagerName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
