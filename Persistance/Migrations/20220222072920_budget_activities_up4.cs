using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistance.Migrations
{
    public partial class budget_activities_up4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectApproval");

            migrationBuilder.CreateTable(
                name: "ProjectBudgetActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProjectNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BudgetNo = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    ApprovedBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BalanceBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SetUser = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SetDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBudgetActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectBudgetActivities_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBudgetActivities_ProjectId",
                table: "ProjectBudgetActivities",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectBudgetActivities");

            migrationBuilder.CreateTable(
                name: "ProjectApproval",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BalanceBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BudgetNo = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ProjectId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProjectNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SetUser = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectApproval_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectApproval_ProjectId",
                table: "ProjectApproval",
                column: "ProjectId");
        }
    }
}
