using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeamPerformancePredictor.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ManagerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    WeekStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoursWorked = table.Column<float>(type: "real", nullable: false),
                    TasksCompleted = table.Column<float>(type: "real", nullable: false),
                    MeetingsAttended = table.Column<float>(type: "real", nullable: false),
                    OvertimeHours = table.Column<float>(type: "real", nullable: false),
                    LeaveDaysTaken = table.Column<float>(type: "real", nullable: false),
                    DeadlinesMissed = table.Column<float>(type: "real", nullable: false),
                    PeerCollaborationScore = table.Column<float>(type: "real", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PredictionResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PredictionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductivityScore = table.Column<float>(type: "real", nullable: false),
                    BurnoutRiskLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BurnoutProbability = table.Column<float>(type: "real", nullable: false),
                    Recommendation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelUsed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredictionResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PredictionResults_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "CreatedAt", "Department", "ManagerName", "TeamName" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Technology", "Rahul Sharma", "Engineering" },
                    { 2, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business", "Priya Mehta", "Marketing" },
                    { 3, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Technology", "Arjun Nair", "Data Science" },
                    { 4, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Strategy", "Sneha Iyer", "Product" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "FullName", "IsActive", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System Admin", true, "$2a$11$pnbPjSTekE6DJg/ZH6ltjuSiEEodgvIFtOHMnFXwTk/pq./UFpZhy", "Admin", "admin" },
                    { 2, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rahul Sharma", true, "$2a$11$ht7TK9w.9c4Cwlt/vKuttuID9LICdqj9n15WahEFhtysUsB0cXn/a", "Manager", "manager" },
                    { 3, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kavita Pillai", true, "$2a$11$naQMYK48RARy1qPSHkv6UuVsEM2Zz1K9NjXpky/7nVSviP4bp39Fm", "HR", "hr" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Department", "Designation", "Email", "IsActive", "JoinDate", "Name", "TeamId" },
                values: new object[,]
                {
                    { 1, "Technology", "Senior Developer", "alice@company.com", true, new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alice Johnson", 1 },
                    { 2, "Technology", "Backend Developer", "bob@company.com", true, new DateTime(2023, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bob Smith", 1 },
                    { 3, "Business", "Marketing Analyst", "carol@company.com", true, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Carol White", 2 },
                    { 4, "Business", "Growth Manager", "david@company.com", true, new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "David Lee", 2 },
                    { 5, "Technology", "Data Scientist", "eva@company.com", true, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Eva Chen", 3 },
                    { 6, "Technology", "ML Engineer", "frank@company.com", true, new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Frank Miller", 3 },
                    { 7, "Strategy", "Product Manager", "grace@company.com", true, new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grace Kim", 4 },
                    { 8, "Strategy", "UX Researcher", "henry@company.com", true, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Henry Brown", 4 }
                });

            migrationBuilder.InsertData(
                table: "ActivityLogs",
                columns: new[] { "Id", "DeadlinesMissed", "EmployeeId", "HoursWorked", "LeaveDaysTaken", "MeetingsAttended", "OvertimeHours", "PeerCollaborationScore", "RecordedAt", "TasksCompleted", "WeekStartDate" },
                values: new object[,]
                {
                    { 1, 0f, 1, 40f, 0f, 5f, 2f, 5f, new DateTime(2024, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 12f, new DateTime(2024, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 0f, 1, 42f, 0f, 6f, 3f, 5f, new DateTime(2024, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 14f, new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 0f, 1, 38f, 1f, 4f, 1f, 4f, new DateTime(2024, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 11f, new DateTime(2024, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 0f, 1, 41f, 0f, 5f, 2f, 5f, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 13f, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 0f, 1, 43f, 0f, 6f, 2f, 5f, new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 15f, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 0f, 1, 40f, 0f, 5f, 2f, 4f, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 12f, new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 1f, 2, 50f, 0f, 7f, 10f, 3f, new DateTime(2024, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 9f, new DateTime(2024, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 2f, 2, 55f, 0f, 8f, 15f, 3f, new DateTime(2024, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 8f, new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 2f, 2, 60f, 0f, 9f, 18f, 2f, new DateTime(2024, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 7f, new DateTime(2024, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 3f, 2, 62f, 0f, 8f, 20f, 2f, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 6f, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 3f, 2, 65f, 0f, 7f, 22f, 2f, new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 5f, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 4f, 2, 68f, 0f, 6f, 25f, 1f, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 4f, new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 1f, 3, 30f, 2f, 3f, 0f, 3f, new DateTime(2024, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 4f, new DateTime(2024, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 2f, 3, 25f, 3f, 2f, 0f, 3f, new DateTime(2024, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 3f, new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 1f, 3, 20f, 2f, 2f, 0f, 2f, new DateTime(2024, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 2f, new DateTime(2024, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 2f, 3, 22f, 3f, 2f, 0f, 2f, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 3f, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 2f, 3, 18f, 2f, 1f, 0f, 2f, new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 2f, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 1f, 3, 20f, 2f, 2f, 0f, 2f, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 2f, new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 0f, 4, 40f, 0f, 5f, 4f, 4f, new DateTime(2024, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 8f, new DateTime(2024, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 0f, 4, 42f, 1f, 5f, 5f, 4f, new DateTime(2024, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 9f, new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 0f, 4, 41f, 0f, 4f, 4f, 3f, new DateTime(2024, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 8f, new DateTime(2024, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 1f, 4, 40f, 0f, 5f, 4f, 4f, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 9f, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, 0f, 4, 42f, 0f, 5f, 5f, 4f, new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 8f, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, 0f, 4, 41f, 0f, 5f, 4f, 3f, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 9f, new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 25, 2f, 5, 32f, 2f, 3f, 1f, 3f, new DateTime(2024, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 5f, new DateTime(2024, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 26, 1f, 5, 35f, 1f, 4f, 2f, 3f, new DateTime(2024, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 7f, new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 27, 1f, 5, 38f, 0f, 4f, 2f, 4f, new DateTime(2024, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 9f, new DateTime(2024, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 28, 0f, 5, 40f, 0f, 5f, 2f, 4f, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 10f, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 29, 0f, 5, 41f, 0f, 5f, 2f, 4f, new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 11f, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 30, 0f, 5, 42f, 0f, 6f, 2f, 5f, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 12f, new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 31, 1f, 6, 45f, 0f, 6f, 6f, 4f, new DateTime(2024, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 10f, new DateTime(2024, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 32, 1f, 6, 48f, 0f, 7f, 8f, 3f, new DateTime(2024, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 9f, new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 33, 2f, 6, 50f, 0f, 7f, 10f, 3f, new DateTime(2024, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 8f, new DateTime(2024, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 34, 2f, 6, 52f, 0f, 6f, 12f, 3f, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 8f, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 35, 1f, 6, 50f, 0f, 7f, 10f, 3f, new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 9f, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 36, 2f, 6, 51f, 0f, 6f, 11f, 3f, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 8f, new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 37, 0f, 7, 40f, 0f, 7f, 1f, 5f, new DateTime(2024, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 13f, new DateTime(2024, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 38, 0f, 7, 42f, 0f, 7f, 2f, 5f, new DateTime(2024, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 15f, new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 39, 0f, 7, 41f, 0f, 6f, 2f, 5f, new DateTime(2024, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 14f, new DateTime(2024, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 40, 0f, 7, 40f, 0f, 7f, 1f, 5f, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 13f, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 41, 0f, 7, 43f, 0f, 8f, 2f, 5f, new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 16f, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 42, 0f, 7, 41f, 0f, 7f, 1f, 5f, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 14f, new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 43, 2f, 8, 38f, 1f, 4f, 0f, 3f, new DateTime(2024, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 6f, new DateTime(2024, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 44, 2f, 8, 39f, 1f, 5f, 1f, 3f, new DateTime(2024, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 7f, new DateTime(2024, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 45, 1f, 8, 40f, 0f, 5f, 1f, 4f, new DateTime(2024, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 8f, new DateTime(2024, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 46, 1f, 8, 40f, 0f, 5f, 1f, 4f, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 9f, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 47, 0f, 8, 41f, 0f, 6f, 1f, 4f, new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 10f, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 48, 0f, 8, 40f, 0f, 5f, 1f, 4f, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 10f, new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_EmployeeId",
                table: "ActivityLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TeamId",
                table: "Employees",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PredictionResults_EmployeeId",
                table: "PredictionResults",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "PredictionResults");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
