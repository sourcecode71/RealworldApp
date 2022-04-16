using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Persistance.Migrations
{
    public partial class prj_status_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "ProjectsStatus",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsStatus_ProjectId",
                table: "ProjectsStatus",
                column: "ProjectId");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropIndex(
                name: "IX_ProjectsStatus_ProjectId",
                table: "ProjectsStatus");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                table: "ProjectsStatus",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectId1",
                table: "ProjectsStatus",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsStatus_ProjectId1",
                table: "ProjectsStatus",
                column: "ProjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectsStatus_Projects_ProjectId1",
                table: "ProjectsStatus",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
