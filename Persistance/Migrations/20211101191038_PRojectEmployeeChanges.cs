using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistance.Migrations
{
    public partial class PRojectEmployeeChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEmployee_AspNetUsers_EmployeeId",
                table: "ProjectEmployee");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEmployee_Projects_ProjectId",
                table: "ProjectEmployee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectEmployee",
                table: "ProjectEmployee");

            migrationBuilder.RenameTable(
                name: "ProjectEmployee",
                newName: "ProjectEmployees");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectEmployee_ProjectId",
                table: "ProjectEmployees",
                newName: "IX_ProjectEmployees_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectEmployee_EmployeeId",
                table: "ProjectEmployees",
                newName: "IX_ProjectEmployees_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectEmployees",
                table: "ProjectEmployees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEmployees_AspNetUsers_EmployeeId",
                table: "ProjectEmployees",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEmployees_Projects_ProjectId",
                table: "ProjectEmployees",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEmployees_AspNetUsers_EmployeeId",
                table: "ProjectEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEmployees_Projects_ProjectId",
                table: "ProjectEmployees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectEmployees",
                table: "ProjectEmployees");

            migrationBuilder.RenameTable(
                name: "ProjectEmployees",
                newName: "ProjectEmployee");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectEmployees_ProjectId",
                table: "ProjectEmployee",
                newName: "IX_ProjectEmployee_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectEmployees_EmployeeId",
                table: "ProjectEmployee",
                newName: "IX_ProjectEmployee_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectEmployee",
                table: "ProjectEmployee",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEmployee_AspNetUsers_EmployeeId",
                table: "ProjectEmployee",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEmployee_Projects_ProjectId",
                table: "ProjectEmployee",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
