using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamPerformancePredictor.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamPredictionAndCodeCommits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "CodeCommits",
                table: "ActivityLogs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "TeamPredictions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    AvgProductivityScore = table.Column<float>(type: "real", nullable: false),
                    AvgBurnoutProbability = table.Column<float>(type: "real", nullable: false),
                    BurnoutRiskLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerformanceGrade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvgHoursWorked = table.Column<float>(type: "real", nullable: false),
                    AvgTasksCompleted = table.Column<float>(type: "real", nullable: false),
                    AvgCollaborationScore = table.Column<float>(type: "real", nullable: false),
                    AvgCodeCommits = table.Column<float>(type: "real", nullable: false),
                    HighRiskEmployeeCount = table.Column<int>(type: "int", nullable: false),
                    TotalEmployees = table.Column<int>(type: "int", nullable: false),
                    TopPerformerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TopPerformerScore = table.Column<float>(type: "real", nullable: false),
                    PredictedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPredictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamPredictions_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CodeCommits",
                value: 25f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CodeCommits",
                value: 25f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 3,
                column: "CodeCommits",
                value: 20f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 4,
                column: "CodeCommits",
                value: 25f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 5,
                column: "CodeCommits",
                value: 25f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 6,
                column: "CodeCommits",
                value: 20f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 7,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 8,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 9,
                column: "CodeCommits",
                value: 10f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 10,
                column: "CodeCommits",
                value: 10f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 11,
                column: "CodeCommits",
                value: 10f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 12,
                column: "CodeCommits",
                value: 5f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 13,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 14,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 15,
                column: "CodeCommits",
                value: 10f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 16,
                column: "CodeCommits",
                value: 10f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 17,
                column: "CodeCommits",
                value: 10f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 18,
                column: "CodeCommits",
                value: 10f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 19,
                column: "CodeCommits",
                value: 20f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 20,
                column: "CodeCommits",
                value: 20f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 21,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 22,
                column: "CodeCommits",
                value: 20f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 23,
                column: "CodeCommits",
                value: 20f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 24,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 25,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 26,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 27,
                column: "CodeCommits",
                value: 20f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 28,
                column: "CodeCommits",
                value: 20f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 29,
                column: "CodeCommits",
                value: 20f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 30,
                column: "CodeCommits",
                value: 25f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 31,
                column: "CodeCommits",
                value: 20f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 32,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 33,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 34,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 35,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 36,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 37,
                column: "CodeCommits",
                value: 25f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 38,
                column: "CodeCommits",
                value: 25f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 39,
                column: "CodeCommits",
                value: 25f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 40,
                column: "CodeCommits",
                value: 25f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 41,
                column: "CodeCommits",
                value: 25f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 42,
                column: "CodeCommits",
                value: 25f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 43,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 44,
                column: "CodeCommits",
                value: 15f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 45,
                column: "CodeCommits",
                value: 20f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 46,
                column: "CodeCommits",
                value: 20f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 47,
                column: "CodeCommits",
                value: 20f);

            migrationBuilder.UpdateData(
                table: "ActivityLogs",
                keyColumn: "Id",
                keyValue: 48,
                column: "CodeCommits",
                value: 20f);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Aaw6AAQaf4DPVP9ojW3dsuXu/1MitoU/YLUT/StE4gCL1HPOBw3gO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$E4lGw3wdB3640Qx4CQzcNeB7lWJMEwLxXDyHuRwCJQycLRBevG6qO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$oJmXcAeNvm9RFbUX2PZBV.MdTGoCNYYnJ2WrLwChB9ccAZH7noiSW");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPredictions_TeamId",
                table: "TeamPredictions",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamPredictions");

            migrationBuilder.DropColumn(
                name: "CodeCommits",
                table: "ActivityLogs");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$pnbPjSTekE6DJg/ZH6ltjuSiEEodgvIFtOHMnFXwTk/pq./UFpZhy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$ht7TK9w.9c4Cwlt/vKuttuID9LICdqj9n15WahEFhtysUsB0cXn/a");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$naQMYK48RARy1qPSHkv6UuVsEM2Zz1K9NjXpky/7nVSviP4bp39Fm");
        }
    }
}
