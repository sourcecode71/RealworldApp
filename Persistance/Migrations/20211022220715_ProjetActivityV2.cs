using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistance.Migrations
{
    public partial class ProjetActivityV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectActivities_AspNetUsers_EmployeeId",
                table: "ProjectActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectActivities_Projects_ProjectId",
                table: "ProjectActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectActivities",
                table: "ProjectActivities");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "ProjectActivities",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "ProjectActivities",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "ProjectActivities",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectActivities",
                table: "ProjectActivities",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectActivities_ProjectId",
                table: "ProjectActivities",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectActivities_AspNetUsers_EmployeeId",
                table: "ProjectActivities",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectActivities_Projects_ProjectId",
                table: "ProjectActivities",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectActivities_AspNetUsers_EmployeeId",
                table: "ProjectActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectActivities_Projects_ProjectId",
                table: "ProjectActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectActivities",
                table: "ProjectActivities");

            migrationBuilder.DropIndex(
                name: "IX_ProjectActivities_ProjectId",
                table: "ProjectActivities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProjectActivities");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "ProjectActivities",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "ProjectActivities",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectActivities",
                table: "ProjectActivities",
                columns: new[] { "ProjectId", "EmployeeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectActivities_AspNetUsers_EmployeeId",
                table: "ProjectActivities",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectActivities_Projects_ProjectId",
                table: "ProjectActivities",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
