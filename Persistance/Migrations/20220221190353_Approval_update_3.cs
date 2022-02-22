using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistance.Migrations
{
    public partial class Approval_update_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectNo",
                table: "ProjectApproval");

            migrationBuilder.AddColumn<bool>(
                name: "IsBudgetApproved",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "SetUser",
                table: "ProjectApproval",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBudgetApproved",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "SetUser",
                table: "ProjectApproval",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectNo",
                table: "ProjectApproval",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
