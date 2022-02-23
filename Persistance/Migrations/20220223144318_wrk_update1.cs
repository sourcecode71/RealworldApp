using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistance.Migrations
{
    public partial class wrk_update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SetUser",
                table: "WorkOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ApprovedBudget",
                table: "WorkOrder",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WorkOrder",
                type: "bit",
                maxLength: 200,
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "SetUser",
                table: "ProjectBudgetActivities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProjectBudgetActivities",
                type: "bit",
                maxLength: 200,
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "SetUser",
                table: "HisWorkOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ApprovedBudget",
                table: "HisWorkOrder",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HisWorkOrder",
                type: "bit",
                maxLength: 200,
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WorkOrder");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProjectBudgetActivities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HisWorkOrder");

            migrationBuilder.AlterColumn<string>(
                name: "SetUser",
                table: "WorkOrder",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedBudget",
                table: "WorkOrder",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "SetUser",
                table: "ProjectBudgetActivities",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SetUser",
                table: "HisWorkOrder",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedBudget",
                table: "HisWorkOrder",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
