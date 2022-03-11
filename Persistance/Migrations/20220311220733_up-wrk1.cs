using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistance.Migrations
{
    public partial class upwrk1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrderEmployee_WorkOrder_WorkOrderId1",
                table: "WorkOrderEmployee");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrderEmployee_WorkOrderId1",
                table: "WorkOrderEmployee");

            migrationBuilder.DropColumn(
                name: "WorkOrderId1",
                table: "WorkOrderEmployee");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkOrderId",
                table: "WorkOrderEmployee",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderEmployee_WorkOrderId",
                table: "WorkOrderEmployee",
                column: "WorkOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrderEmployee_WorkOrder_WorkOrderId",
                table: "WorkOrderEmployee",
                column: "WorkOrderId",
                principalTable: "WorkOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrderEmployee_WorkOrder_WorkOrderId",
                table: "WorkOrderEmployee");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrderEmployee_WorkOrderId",
                table: "WorkOrderEmployee");

            migrationBuilder.AlterColumn<string>(
                name: "WorkOrderId",
                table: "WorkOrderEmployee",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkOrderId1",
                table: "WorkOrderEmployee",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderEmployee_WorkOrderId1",
                table: "WorkOrderEmployee",
                column: "WorkOrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrderEmployee_WorkOrder_WorkOrderId1",
                table: "WorkOrderEmployee",
                column: "WorkOrderId1",
                principalTable: "WorkOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
