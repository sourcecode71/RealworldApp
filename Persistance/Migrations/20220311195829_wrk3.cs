using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistance.Migrations
{
    public partial class wrk3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkOrderEmployee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkOrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkOrderId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BudgetHours = table.Column<double>(type: "float", nullable: false),
                    TotalHourLog = table.Column<double>(type: "float", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EmployeeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderEmployee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrderEmployee_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkOrderEmployee_WorkOrder_WorkOrderId1",
                        column: x => x.WorkOrderId1,
                        principalTable: "WorkOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderEmployee_EmployeeId",
                table: "WorkOrderEmployee",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderEmployee_WorkOrderId1",
                table: "WorkOrderEmployee",
                column: "WorkOrderId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkOrderEmployee");
        }
    }
}
