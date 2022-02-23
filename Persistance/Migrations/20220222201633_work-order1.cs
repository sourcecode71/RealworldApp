using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistance.Migrations
{
    public partial class workorder1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkOrderNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ProjectId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProjectNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ApprovedBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OTDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SetUser = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SetDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrder_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HisWorkOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkOrderNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProjectNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ApprovedBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OTDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SetUser = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SetDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HisWorkOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HisWorkOrder_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HisWorkOrder_WorkOrder_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HisWorkOrder_ProjectId",
                table: "HisWorkOrder",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_HisWorkOrder_WorkOrderId",
                table: "HisWorkOrder",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrder_ProjectId",
                table: "WorkOrder",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HisWorkOrder");

            migrationBuilder.DropTable(
                name: "WorkOrder");
        }
    }
}
