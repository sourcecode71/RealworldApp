using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistance.Migrations
{
    public partial class inv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Projects_ProjectId1",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_ProjectId1",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "Invoice");

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "WorkOrder",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsInvoiced",
                table: "WorkOrder",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "Invoice",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "Invoice",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ProjectId",
                table: "Invoice",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Projects_ProjectId",
                table: "Invoice",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Projects_ProjectId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_ProjectId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "WorkOrder");

            migrationBuilder.DropColumn(
                name: "IsInvoiced",
                table: "WorkOrder");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Invoice");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                table: "Invoice",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectId1",
                table: "Invoice",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ProjectId1",
                table: "Invoice",
                column: "ProjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Projects_ProjectId1",
                table: "Invoice",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
