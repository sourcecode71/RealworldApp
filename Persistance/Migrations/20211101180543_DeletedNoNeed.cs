using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistance.Migrations
{
    public partial class DeletedNoNeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ////migrationBuilder.DropTable(
            ////    name: "EmployeeProject");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "EmployeeProject",
            //    columns: table => new
            //    {
            //        EmployeesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ProjectsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_EmployeeProject", x => new { x.EmployeesId, x.ProjectsId });
            //        table.ForeignKey(
            //            name: "FK_EmployeeProject_AspNetUsers_EmployeesId",
            //            column: x => x.EmployeesId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_EmployeeProject_Projects_ProjectsId",
            //            column: x => x.ProjectsId,
            //            principalTable: "Projects",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_EmployeeProject_ProjectsId",
            //    table: "EmployeeProject",
            //    column: "ProjectsId");
        }
    }
}
