using Application.DTOs;
using Application.Core.Projects;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.Enums;
using Application.Core.Employees;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;
using System.Drawing;
using System;

namespace Web.ApiControllers
{
    public class ProjectController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ProjectDto>>> GetProjects()
        {
            return Ok(await Mediator.Send(new List.Query()));
        }

        [HttpGet("byId")]
        public async Task<ActionResult<ProjectDto>> GetProjectById(int selfProjectId)
        {
            return await Mediator.Send(new GetById.Query { SelfProjectId = selfProjectId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectDto project)
        {
            Dictionary<EmployeeType, string> employees = new Dictionary<EmployeeType, string>();

            employees.Add(EmployeeType.Engineering, project.Engineering);
            employees.Add(EmployeeType.Drawing, project.Drawing);
            employees.Add(EmployeeType.Approval, project.Approval);

            Project projectDomain = new Project
            {
                Balance = project.Budget,
                Factor = project.Budget / (double)project.Schedule,
                Client = project.Client,
                DeliveryDate = project.DeliveryDate,
                Paid = 0,
                Name = project.Name,
                EStatus = project.EStatus,
                Progress = 0,
                Schedule = project.Schedule,
                Status = ProjectStatus.OnTime,
                CreatedDate = DateTime.Now,
                Budget = project.Budget
            };

            return Ok(await Mediator.Send(new Create.Command { Project = projectDomain, Employees = employees }));
        }

        [HttpPost("archive")]
        public async Task<IActionResult> ArchiveProject(ProjectDto project)
        {
            return Ok(await Mediator.Send(new Archive.Command { SelfProjectId = project.SelfProjectId }));
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteProject(Complete.Command project)
        {
            return Ok(await Mediator.Send(project));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            return Ok(await Mediator.Send(new Delete.Command { Id = id }));
        }
        [HttpPut("delayed")]
        public async Task<IActionResult> AddDelayedComment(string delayed, int selfProjectId)
        {
            return Ok(await Mediator.Send(new AddDelayedComment.Command { Delayed = delayed, SelfProjectId = selfProjectId }));
        }

        [HttpPut("modified")]
        public async Task<IActionResult> AddModifiedComment(string modified, int selfProjectId)
        {
            return Ok(await Mediator.Send(new AddModifiedComment.Command { Modified = modified, SelfProjectId = selfProjectId }));
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignEmployee(ProjectDto details)
        {
            string[] names = details.EmployeesNames.Split(',');
            List<string> employeesNames = new List<string>();

            foreach (var name in names)
            {
                employeesNames.Add(name.Replace(" ", string.Empty));
            }

            return Ok(await Mediator.Send(new AddEmployee.Command { EmployeesEmails = employeesNames, SelfProjectId = details.SelfProjectId }));
        }

        [HttpPost("savedelayed")]
        public async Task<IActionResult> SaveDelayedComment(ProjectDto details)
        {
            return Ok(await Mediator.Send(new AddDelayedComment.Command { Delayed = details.AdminDelayedComment, SelfProjectId = details.SelfProjectId }));
        }

        [HttpPost("savemodified")]
        public async Task<IActionResult> SaveModifiedComment(ProjectDto details)
        {
            return Ok(await Mediator.Send(new AddModifiedComment.Command { Modified = details.AdminModifiedComment, Budget = details.Budget, SelfProjectId = details.SelfProjectId }));
        }


        [HttpPost("activity")]
        public async Task<ActionResult> AddActivity(ProjectActivityDto projectActivity)
        {
            return Ok(await Mediator.Send(new AddActivity.Command
            {
                EmployeeEmail = projectActivity.EmployeeEmail,
                SelfProjectId = projectActivity.SelfProjectId,
                Duration = projectActivity.Duration,
                Comment = projectActivity.Comment,
                Status = (ProjectStatus)projectActivity.Status,
                StatusComment = projectActivity.StatusComment
            }));
        }

        [HttpPost("save-paid")]
        public async Task<ActionResult> SavePaid(ProjectDto project)
        {
            return Ok(await Mediator.Send(new SavePaid.Command
            {
                SelfProjectId = project.SelfProjectId,
                Paid = project.Paid
            }));
        }

        [HttpGet("get-excel-report")]
        public FileResult ExportExcel(int projectType)
        {
            FileStream export = CreateExcel(projectType);
            return File(export, "application/octet-stream", "project_reports.xlsx");
        }

        private FileStream CreateExcel(int projectStatus)
        {
            var reportsFolder = @"C:\project-reports\\";

            List<ProjectDto> projects = Mediator.Send(new ListByProjectStatus.Query { ProjectStatus = projectStatus }).Result;

            string fileName = "Project report";
            string newPath = reportsFolder + fileName + ".xlsx";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage package = new ExcelPackage();
            ExcelWorkbook workbook = package.Workbook;
            ExcelWorksheet worksheet = workbook.Worksheets.Add("MID");

            worksheet.Cells["A1:D1"].Merge = true;
            worksheet.Cells["A1:D1"].Value = "PROJECT FOLLOW UP - CONTROL SHEET";
            worksheet.Cells["A1:D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A1:D1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["A1:D1"].Style.Font.Size = 22;
            worksheet.Cells["A1:D1"].Style.Font.Bold = true;

            worksheet.Cells["O4:Q4"].Merge = true;
            worksheet.Cells["O4:Q4"].Value = "COMENTARIOS";
            worksheet.Cells["O4:Q4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["O4:Q4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["O4:Q4"].Style.Font.Size = 10;

            worksheet.Cells["O5:Q5"].Merge = true;
            worksheet.Cells["O5:Q5"].Value = "STAFF COMMENTS";
            worksheet.Cells["O5:Q5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["O5:Q5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["O5:Q5"].Style.Font.Size = 10;

            worksheet.Cells["A6"].Value = "FOLIO";
            worksheet.Cells["A6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["A6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["A6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["A6"].Style.Font.Size = 10;
            worksheet.Cells["A6"].Style.Font.Bold = true;

            worksheet.Cells["B6"].Value = "PROYECTO";
            worksheet.Cells["B6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["B6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["B6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["B6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["B6"].Style.Font.Size = 10;
            worksheet.Cells["B6"].Style.Font.Bold = true;

            worksheet.Cells["C6"].Value = "CLIENTE";
            worksheet.Cells["C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["C6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["C6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["C6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["C6"].Style.Font.Size = 10;
            worksheet.Cells["C6"].Style.Font.Bold = true;

            worksheet.Cells["D6"].Value = "INGENIERÍA";
            worksheet.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["D6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["D6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["D6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["D6"].Style.Font.Size = 10;
            worksheet.Cells["D6"].Style.Font.Bold = true;

            worksheet.Cells["E6"].Value = "DIBUJO";
            worksheet.Cells["E6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["E6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["E6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["E6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["E6"].Style.Font.Size = 10;
            worksheet.Cells["E6"].Style.Font.Bold = true;

            worksheet.Cells["F6"].Value = "VISTO BUENO";
            worksheet.Cells["F6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["F6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["F6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["F6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["F6"].Style.Font.Size = 10;
            worksheet.Cells["F6"].Style.Font.Bold = true;

            worksheet.Cells["G6"].Value = "FECHA DE ENTREGA";
            worksheet.Cells["G6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["G6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["G6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["G6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["G6"].Style.Font.Size = 10;
            worksheet.Cells["G6"].Style.Font.Bold = true;

            worksheet.Cells["H6"].Value = "PROGRAMA";
            worksheet.Cells["H6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["H6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["H6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["H6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["H6"].Style.Font.Size = 10;
            worksheet.Cells["H6"].Style.Font.Bold = true;

            worksheet.Cells["I6"].Value = "SEMANA DE TRABAJO";
            worksheet.Cells["I6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["I6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["I6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["I6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["I6"].Style.Font.Size = 10;
            worksheet.Cells["I6"].Style.Font.Bold = true;

            worksheet.Cells["J6"].Value = "PRESUPUESTO";
            worksheet.Cells["J6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["J6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["J6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["J6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["J6"].Style.Font.Size = 10;
            worksheet.Cells["J6"].Style.Font.Bold = true;

            worksheet.Cells["K6"].Value = "PAGADO";
            worksheet.Cells["K6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["K6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["K6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["K6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["K6"].Style.Font.Size = 10;
            worksheet.Cells["K6"].Style.Font.Bold = true;

            worksheet.Cells["L6"].Value = "POR PAGAR";
            worksheet.Cells["L6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["L6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["L6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["L6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["L6"].Style.Font.Size = 10;
            worksheet.Cells["L6"].Style.Font.Bold = true;

            worksheet.Cells["M6"].Value = "FACTOR";
            worksheet.Cells["M6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["M6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["M6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["M6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["M6"].Style.Font.Size = 10;
            worksheet.Cells["M6"].Style.Font.Bold = true;

            worksheet.Cells["N6"].Value = "ESTATUS";
            worksheet.Cells["N6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["N6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["N6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["N6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["N6"].Style.Font.Size = 10;
            worksheet.Cells["N6"].Style.Font.Bold = true;

            worksheet.Cells["O6"].Value = "INGENIERÍA";
            worksheet.Cells["O6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["O6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["O6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["O6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["O6"].Style.Font.Size = 10;
            worksheet.Cells["O6"].Style.Font.Bold = true;

            worksheet.Cells["P6"].Value = "DIBUJO";
            worksheet.Cells["P6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["P6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["P6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["P6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["P6"].Style.Font.Size = 10;
            worksheet.Cells["P6"].Style.Font.Bold = true;

            worksheet.Cells["Q6"].Value = "ADMINISTRACION";
            worksheet.Cells["Q6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["Q6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["Q6"].Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            worksheet.Cells["Q6"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["Q6"].Style.Font.Size = 10;
            worksheet.Cells["Q6"].Style.Font.Bold = true;

            worksheet.Cells["A6:Q20"].AutoFitColumns();

            int i = 0;
            int cellNumber = 8 + i;

            foreach (var project in projects)
            {
                string cellId = "A" + cellNumber;
                string cellProject = "B" + cellNumber;
                string cellClient = "C" + cellNumber;
                string cellEng = "D" + cellNumber;
                string cellDraw = "E" + cellNumber;
                string cellApp = "F" + cellNumber;
                string cellDelivery = "G" + cellNumber;
                string cellSchedule = "H" + cellNumber;
                string cellProgress = "I" + cellNumber;
                string cellBudget = "J" + cellNumber;
                string cellPaid = "K" + cellNumber;
                string cellBalance = "L" + cellNumber;
                string cellFactor = "M" + cellNumber;
                string cellStatus = "N" + cellNumber;
                string cellComEng = "O" + cellNumber;
                string cellComDraw = "P" + cellNumber;
                string cellComAdmin = "Q" + cellNumber;

                worksheet.Cells[cellId].Value = project.SelfProjectId;
                worksheet.Cells[cellProject].Value = project.Name;
                worksheet.Cells[cellClient].Value = project.Client;
                worksheet.Cells[cellEng].Value = project.Engineering;
                worksheet.Cells[cellDraw].Value = project.Drawing;
                worksheet.Cells[cellApp].Value = project.Approval;
                worksheet.Cells[cellDelivery].Value = project.DeliveryDate.ToString("MM/dd/yyyy");
                worksheet.Cells[cellSchedule].Value = project.Schedule;
                worksheet.Cells[cellProgress].Value = project.Progress;
                worksheet.Cells[cellBudget].Value = project.Budget;
                worksheet.Cells[cellPaid].Value = project.Paid;
                worksheet.Cells[cellBalance].Value = project.Balance;
                worksheet.Cells[cellFactor].Value = project.Factor;
                worksheet.Cells[cellStatus].Value = project.EStatus;

                if (projectStatus == 0 || projectStatus == 3) //Delayed
                {
                    worksheet.Cells[cellComEng].Value = project.EmployeeDelayedComment;
                    worksheet.Cells[cellComDraw].Value = project.EmployeeDelayedComment;
                    worksheet.Cells[cellComAdmin].Value = project.AdminDelayedComment;
                }
                else if (projectStatus == 0 || projectStatus == 4) //Modified
                {
                    worksheet.Cells[cellComEng].Value = project.EmployeeModifiedComment;
                    worksheet.Cells[cellComDraw].Value = project.EmployeeModifiedComment;
                    worksheet.Cells[cellComAdmin].Value = project.AdminModifiedComment;
                }
            }

            package.SaveAs(new FileInfo(newPath));
            package.Dispose();

            var fileStream = System.IO.File.OpenRead(newPath);

            return fileStream;
        }
    }
}