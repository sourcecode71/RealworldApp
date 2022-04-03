using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistance.Migrations
{
    public partial class wrkUP3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WorkOrderEmployee",
                type: "bit",
                maxLength: 200,
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "OriginalBHours",
                table: "WorkOrderEmployee",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SetDate",
                table: "WorkOrderEmployee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SetUser",
                table: "WorkOrderEmployee",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WorkOrderEmployee");

            migrationBuilder.DropColumn(
                name: "OriginalBHours",
                table: "WorkOrderEmployee");

            migrationBuilder.DropColumn(
                name: "SetDate",
                table: "WorkOrderEmployee");

            migrationBuilder.DropColumn(
                name: "SetUser",
                table: "WorkOrderEmployee");
        }
    }
}
