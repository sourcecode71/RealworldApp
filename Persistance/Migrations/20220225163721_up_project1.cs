using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistance.Migrations
{
    public partial class up_project1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Clients_ClientId1",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_ClientId1",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "IsBudgetApproved",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "ProjectBudgetActivities");

            migrationBuilder.DropColumn(
                name: "ClientId1",
                table: "Company");

            migrationBuilder.AddColumn<double>(
                name: "ApprovedBudget",
                table: "Projects",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BudgetApprovedDate",
                table: "Projects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "BudgetApprovedStatus",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "BudgetSubmitDate",
                table: "Projects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<double>(
                name: "BalanceBudget",
                table: "ProjectBudgetActivities",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ApprovedBudget",
                table: "ProjectBudgetActivities",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalSetUser",
                table: "ProjectBudgetActivities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Budget",
                table: "ProjectBudgetActivities",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "BudgetSubmitDate",
                table: "ProjectBudgetActivities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProjectBudgetActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "ClientId",
                table: "Company",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_ClientId",
                table: "Company",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Clients_ClientId",
                table: "Company",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Clients_ClientId",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_ClientId",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "ApprovedBudget",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "BudgetApprovedDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "BudgetApprovedStatus",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "BudgetSubmitDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ApprovalSetUser",
                table: "ProjectBudgetActivities");

            migrationBuilder.DropColumn(
                name: "Budget",
                table: "ProjectBudgetActivities");

            migrationBuilder.DropColumn(
                name: "BudgetSubmitDate",
                table: "ProjectBudgetActivities");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProjectBudgetActivities");

            migrationBuilder.AddColumn<bool>(
                name: "IsBudgetApproved",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "BalanceBudget",
                table: "ProjectBudgetActivities",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedBudget",
                table: "ProjectBudgetActivities",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ApprovalStatus",
                table: "ProjectBudgetActivities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId1",
                table: "Company",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_ClientId1",
                table: "Company",
                column: "ClientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Clients_ClientId1",
                table: "Company",
                column: "ClientId1",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
