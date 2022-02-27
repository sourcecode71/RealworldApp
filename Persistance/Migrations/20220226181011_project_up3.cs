using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistance.Migrations
{
    public partial class project_up3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalHourLog",
                table: "ProjectEmployees",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Hourlogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectsId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EmpId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectEmployeesId = table.Column<int>(type: "int", nullable: true),
                    SpentHour = table.Column<double>(type: "float", nullable: false),
                    BalanceHour = table.Column<double>(type: "float", nullable: false),
                    SpentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hourlogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hourlogs_ProjectEmployees_ProjectEmployeesId",
                        column: x => x.ProjectEmployeesId,
                        principalTable: "ProjectEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hourlogs_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hourlogs_ProjectEmployeesId",
                table: "Hourlogs",
                column: "ProjectEmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_Hourlogs_ProjectsId",
                table: "Hourlogs",
                column: "ProjectsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hourlogs");

            migrationBuilder.DropColumn(
                name: "TotalHourLog",
                table: "ProjectEmployees");
        }
    }
}
