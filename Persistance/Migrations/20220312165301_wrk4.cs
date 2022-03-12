using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistance.Migrations
{
    public partial class wrk4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BudgetStatus",
                table: "WorkOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "BudgetUpdateDate",
                table: "WorkOrder",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "HisBudgetActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BudgetFor = table.Column<int>(type: "int", nullable: false),
                    BudgetNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalBudget = table.Column<double>(type: "float", nullable: false),
                    ChangedBudget = table.Column<double>(type: "float", nullable: false),
                    BudgetSubmitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    OriginalSetUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalSetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", maxLength: 200, nullable: false),
                    SetUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HisBudgetActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HisBudgetActivities_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HisBudgetActivities_WorkOrder_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HisBudgetActivities_ProjectId",
                table: "HisBudgetActivities",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_HisBudgetActivities_WorkOrderId",
                table: "HisBudgetActivities",
                column: "WorkOrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HisBudgetActivities");

            migrationBuilder.DropColumn(
                name: "BudgetStatus",
                table: "WorkOrder");

            migrationBuilder.DropColumn(
                name: "BudgetUpdateDate",
                table: "WorkOrder");
        }
    }
}
