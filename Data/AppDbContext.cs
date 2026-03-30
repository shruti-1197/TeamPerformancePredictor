using Microsoft.EntityFrameworkCore;
using TeamPerformancePredictor.Models;

namespace TeamPerformancePredictor.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<PredictionResult> PredictionResults { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<TeamPredictionResult> TeamPredictions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ---- Seed Teams ----
            modelBuilder.Entity<Team>().HasData(
                new Team { Id = 1, TeamName = "Engineering", Department = "Technology", ManagerName = "Rahul Sharma", CreatedAt = new DateTime(2024, 1, 15) },
                new Team { Id = 2, TeamName = "Marketing", Department = "Business", ManagerName = "Priya Mehta", CreatedAt = new DateTime(2024, 1, 15) },
                new Team { Id = 3, TeamName = "Data Science", Department = "Technology", ManagerName = "Arjun Nair", CreatedAt = new DateTime(2024, 2, 1) },
                new Team { Id = 4, TeamName = "Product", Department = "Strategy", ManagerName = "Sneha Iyer", CreatedAt = new DateTime(2024, 2, 1) }
            );

            // ---- Seed Users ----
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser { Id = 1, Username = "admin",   FullName = "System Admin",    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),   Role = "Admin",   CreatedAt = new DateTime(2024, 1, 1) },
                new ApplicationUser { Id = 2, Username = "manager", FullName = "Rahul Sharma",    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager@123"), Role = "Manager", CreatedAt = new DateTime(2024, 1, 15) },
                new ApplicationUser { Id = 3, Username = "hr",      FullName = "Kavita Pillai",   PasswordHash = BCrypt.Net.BCrypt.HashPassword("HR@123"),      Role = "HR",      CreatedAt = new DateTime(2024, 1, 15) }
            );

            // ---- Seed Employees ----
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1,  Name = "Alice Johnson",   Email = "alice@company.com",   Department = "Technology", Designation = "Senior Developer",     TeamId = 1, JoinDate = new DateTime(2023, 6, 1),  IsActive = true },
                new Employee { Id = 2,  Name = "Bob Smith",       Email = "bob@company.com",     Department = "Technology", Designation = "Backend Developer",    TeamId = 1, JoinDate = new DateTime(2023, 9, 15), IsActive = true },
                new Employee { Id = 3,  Name = "Carol White",     Email = "carol@company.com",   Department = "Business",   Designation = "Marketing Analyst",    TeamId = 2, JoinDate = new DateTime(2024, 1, 10), IsActive = true },
                new Employee { Id = 4,  Name = "David Lee",       Email = "david@company.com",   Department = "Business",   Designation = "Growth Manager",       TeamId = 2, JoinDate = new DateTime(2023, 5, 20), IsActive = true },
                new Employee { Id = 5,  Name = "Eva Chen",        Email = "eva@company.com",     Department = "Technology", Designation = "Data Scientist",       TeamId = 3, JoinDate = new DateTime(2024, 2, 1),  IsActive = true },
                new Employee { Id = 6,  Name = "Frank Miller",    Email = "frank@company.com",   Department = "Technology", Designation = "ML Engineer",          TeamId = 3, JoinDate = new DateTime(2023, 11, 1), IsActive = true },
                new Employee { Id = 7,  Name = "Grace Kim",       Email = "grace@company.com",   Department = "Strategy",   Designation = "Product Manager",      TeamId = 4, JoinDate = new DateTime(2023, 8, 1),  IsActive = true },
                new Employee { Id = 8,  Name = "Henry Brown",     Email = "henry@company.com",   Department = "Strategy",   Designation = "UX Researcher",        TeamId = 4, JoinDate = new DateTime(2024, 1, 20), IsActive = true }
            );

            // ---- Seed Activity Logs (6 weeks per employee — richer ML training data) ----
            int logId = 1;
            var baseDate = new DateTime(2024, 11, 4); // Start 6 weeks ago

            // Alice — Consistently high performer
            float[][] aliceData = {
                new float[] {40,12,5,2,0,0,5},
                new float[] {42,14,6,3,0,0,5},
                new float[] {38,11,4,1,1,0,4},
                new float[] {41,13,5,2,0,0,5},
                new float[] {43,15,6,2,0,0,5},
                new float[] {40,12,5,2,0,0,4}
            };

            // Bob — Burnout trajectory (escalating overtime)
            float[][] bobData = {
                new float[] {50,9,7,10,0,1,3},
                new float[] {55,8,8,15,0,2,3},
                new float[] {60,7,9,18,0,2,2},
                new float[] {62,6,8,20,0,3,2},
                new float[] {65,5,7,22,0,3,2},
                new float[] {68,4,6,25,0,4,1}
            };

            // Carol — Low productivity / disengaged
            float[][] carolData = {
                new float[] {30,4,3,0,2,1,3},
                new float[] {25,3,2,0,3,2,3},
                new float[] {20,2,2,0,2,1,2},
                new float[] {22,3,2,0,3,2,2},
                new float[] {18,2,1,0,2,2,2},
                new float[] {20,2,2,0,2,1,2}
            };

            // David — Average, stable
            float[][] davidData = {
                new float[] {40,8,5,4,0,0,4},
                new float[] {42,9,5,5,1,0,4},
                new float[] {41,8,4,4,0,0,3},
                new float[] {40,9,5,4,0,1,4},
                new float[] {42,8,5,5,0,0,4},
                new float[] {41,9,5,4,0,0,3}
            };

            // Eva — Improving trend
            float[][] evaData = {
                new float[] {32,5,3,1,2,2,3},
                new float[] {35,7,4,2,1,1,3},
                new float[] {38,9,4,2,0,1,4},
                new float[] {40,10,5,2,0,0,4},
                new float[] {41,11,5,2,0,0,4},
                new float[] {42,12,6,2,0,0,5}
            };

            // Frank — Medium risk, borderline
            float[][] frankData = {
                new float[] {45,10,6,6,0,1,4},
                new float[] {48,9,7,8,0,1,3},
                new float[] {50,8,7,10,0,2,3},
                new float[] {52,8,6,12,0,2,3},
                new float[] {50,9,7,10,0,1,3},
                new float[] {51,8,6,11,0,2,3}
            };

            // Grace — Star performer
            float[][] graceData = {
                new float[] {40,13,7,1,0,0,5},
                new float[] {42,15,7,2,0,0,5},
                new float[] {41,14,6,2,0,0,5},
                new float[] {40,13,7,1,0,0,5},
                new float[] {43,16,8,2,0,0,5},
                new float[] {41,14,7,1,0,0,5}
            };

            // Henry — New joiner, ramping up
            float[][] henryData = {
                new float[] {38,6,4,0,1,2,3},
                new float[] {39,7,5,1,1,2,3},
                new float[] {40,8,5,1,0,1,4},
                new float[] {40,9,5,1,0,1,4},
                new float[] {41,10,6,1,0,0,4},
                new float[] {40,10,5,1,0,0,4}
            };

            var allEmployeeData = new float[][][] { aliceData, bobData, carolData, davidData, evaData, frankData, graceData, henryData };

            for (int empIdx = 0; empIdx < 8; empIdx++)
            {
                for (int week = 0; week < 6; week++)
                {
                    var d = allEmployeeData[empIdx][week];
                    modelBuilder.Entity<ActivityLog>().HasData(new ActivityLog
                    {
                        Id = logId++,
                        EmployeeId = empIdx + 1,
                        WeekStartDate = baseDate.AddDays(week * 7),
                        HoursWorked = d[0],
                        TasksCompleted = d[1],
                        MeetingsAttended = d[2],
                        OvertimeHours = d[3],
                        LeaveDaysTaken = d[4],
                        DeadlinesMissed = d[5],
                        PeerCollaborationScore = d[6],
                        CodeCommits = d[6] * 5, // estimated commits based on collaboration
                        RecordedAt = baseDate.AddDays(week * 7 + 5)
                    });
                }
            }
        }
    }
}
