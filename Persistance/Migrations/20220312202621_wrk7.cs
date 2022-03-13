using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistance.Migrations
{
    public partial class wrk7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HourLogFor",
                table: "Hourlogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkOrderId",
                table: "Hourlogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Hourlogs_WorkOrderId",
                table: "Hourlogs",
                column: "WorkOrderId");

       
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hourlogs_WorkOrder_WorkOrderId",
                table: "Hourlogs");

            migrationBuilder.DropIndex(
                name: "IX_Hourlogs_WorkOrderId",
                table: "Hourlogs");

            migrationBuilder.DropColumn(
                name: "HourLogFor",
                table: "Hourlogs");

            migrationBuilder.DropColumn(
                name: "WorkOrderId",
                table: "Hourlogs");
        }
    }
}
