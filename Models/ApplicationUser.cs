using System.ComponentModel.DataAnnotations;

namespace TeamPerformancePredictor.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // "Admin" | "Manager" | "HR"
        public string Role { get; set; } = "HR";

        public string? FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;
    }
}
