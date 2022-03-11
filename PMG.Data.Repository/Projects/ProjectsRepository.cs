using Application.DTOs;
using Domain;
using Domain.Enums;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using PMG.Data.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Projects
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly DataContext _context;
        public ProjectsRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public string GetProjectNumber(ProjectDto projectDto)
        {
            DateTime CurrentDate = DateTime.Now;
            var TodayPmCount = _context.Projects.Where(P=>P.CreatedDate.Date == CurrentDate.Date ).Count()+1;

            string Day = CurrentDate.Day.ToString("00");
            string Month = CurrentDate.Month.ToString("00");
            string Year = CurrentDate.Year.ToString();
            string ProjectNumber = string.Format("{0}{1}{2}{3}", Day, Month, Year, TodayPmCount.ToString("00"));

            return ProjectNumber;
        }

        public async Task< List<ProjectDto>> GetProjectBySearch(string SearchTag)
        {
            var projects = await _context.Projects.Join(_context.Clients,
                prj =>prj.ClientId,
                cln=>cln.Id,(prj,cln) =>new { Proj =prj,client=cln})
                .Where(pp => pp.Proj.Name.Contains(SearchTag)).Take(20).Select(pp=> new
                ProjectDto()
                {
                    Id = pp.Proj.Id,
                    Name = pp.Proj.Name,
                    Client=pp.client.Name,
                    Budget=pp.Proj.Budget,
                    Balance =pp.Proj.Balance,
                    DeliveryDate=pp.Proj.DeliveryDate,
                    Description =pp.Proj.Description,
                    Year = pp.Proj.Year,
                    ProjectNo = pp.Proj.ProjectNo
                }).ToListAsync();
           
            return projects;
        }

       

        public async Task<bool> SubmitBudget(ProjectApprovalDto dto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string PmBudgetNo = GetPmBudgetNumber(dto);

                    var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == dto.Id);

                    if (project == null)
                    {
                        return false;
                    }

                    project.BudgetApprovedStatus = 0;
                    project.BudgetSubmitDate = DateTime.Now;
                    project.Budget = dto.Budget;

                    var pmApproval = new ProjectBudgetActivities
                    {
                        Id = Guid.NewGuid(),
                        BudgetNo = PmBudgetNo,
                        ProjectId = dto.Id,
                        ProjectNo = dto.ProjectNo,
                        Budget = dto.Budget,
                        Comments = dto.Comments,
                        BudgetSubmitDate = DateTime.Now,
                        ApprovalSetUser = dto.ApprovalSetUser
                    };

                    _context.Add(pmApproval);

                    var Status = await _context.SaveChangesAsync();
                   await transaction.CommitAsync();

                    return Status == 1;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }

        public async Task<bool> ApprovalBudget(ProjectApprovalDto dto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string PmBudgetNo = GetPmBudgetNumber(dto);

                    var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == dto.Id);

                    if (project != null)
                    {
                        project.BudgetApprovedStatus = dto.Status;
                        project.BudgetApprovedDate = DateTime.Now;
                        project.ApprovedBudget = dto.ApprovedBudget;
                    }
                   
                    var pba = await _context.ProjectBudgetActivities.FirstOrDefaultAsync(p => p.BudgetNo == dto.BudegtNo);
                    if(pba != null)
                    {
                        pba.Status = dto.Status;
                        pba.ApprovedDate = DateTime.Now;
                        pba.BudgetSubmitDate = pba.BudgetSubmitDate;
                        pba.ApprovedBudget= dto.ApprovedBudget;
                        pba.Comments    = dto.Comments;
                        pba.ApprovalSetUser = dto.ApprovalSetUser;
                    }
                    
                    var Status = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Status == 1;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }

        public async Task<List<ProjectApprovalDto>> LoadProjectBudgetAcitivies(string projectName)
        {
            try
            {
                var prjoect = (from pa in _context.ProjectBudgetActivities
                               join pj in _context.Projects on pa.ProjectId equals pj.Id
                               orderby pa.BudgetSubmitDate descending
                               select new ProjectApprovalDto
                               {
                                   Id = pa.Id.ToString(),
                                   ProjectNo = pj.ProjectNo,
                                   BudegtNo = pa.BudgetNo,
                                   Budget = pa.Budget,
                                   ClientName = pj.Client,
                                   ApprovalStatus = pa.Status,
                                   ApprovedBudget = pj.ApprovedBudget,
                                   BudgetSubmitDateStr = pa.BudgetSubmitDate.ToString("dd/MM/yyyy"),
                                   ProjectName = pj.Name,
                                   Balance = pj.Balance,
                                   ProjectId = new Guid(pj.Id),
                                   ApprovalStatusStr = pa.Status ==0 ? "Waiting" : pa.Status == 1 ? "Approved" : "Not Approved",
                                   ApprovalDateStr = pa.ApprovedDate.ToString("dd/MM/yyyy")


                               }).Take(50).ToListAsync();

                return await prjoect;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<ClientDTO>> GetAllClient()
        {
            try
            {
                var clDTO = await (from cl in _context.Clients
                             where cl.IsActive == true
                             select new ClientDTO { Name = cl.Name, Id = cl.Id, Address = cl.Address }).
                             ToListAsync();
                return clDTO;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<ProjectDto>> GetAllProjects(string empId)
        {
            try
            {
                var projects = await (from prj in _context.Projects
                               join emw in _context.ProjectEmployees on prj.Id equals emw.ProjectId
                               join cli in _context.Clients on prj.ClientId equals cli.Id into cliList
                               from cts in cliList.DefaultIfEmpty()
                                where emw.EmployeeId == empId && prj.Status != ProjectStatus.Completed
                               select new ProjectDto
                               {
                                   Name = prj.Name,
                                   Id = prj.Id,
                                   ProjectNo = prj.ProjectNo,
                                   Year = prj.Year,
                                   Budget = prj.Budget,
                                   Description = prj.Description,
                                   ClientName = cts.Name,
                                   
                               }).Distinct().ToListAsync();

                return projects;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<ProjectDto>> GetAllActiveProjects()
        {
            try
            {
                var projects = await (from prj in _context.Projects
                                     join emw in _context.ProjectEmployees on prj.Id equals emw.ProjectId
                                     join cli in _context.Clients on prj.ClientId equals cli.Id into cliList
                                     from cts in cliList.DefaultIfEmpty()
                                     join pa in _context.ProjectBudgetActivities on prj.Id equals pa.ProjectId into paActs
                                     from paAct in paActs.DefaultIfEmpty()
                                     where  prj.Status != ProjectStatus.Completed
                                     select new ProjectDto
                                     {
                                         Name = prj.Name,
                                         Id = prj.Id,
                                         ProjectNo = prj.ProjectNo,
                                         Year = prj.Year,
                                         Budget = prj.Budget,
                                         Description = prj.Description,
                                         ClientName = cts.Name,
                                         BudgetApprovalStr = paAct.Status == 0 ? "Waiting" : paAct.Status == 1 ? "Approved" : "Not Approved",
                                         Status = EnumConverter.ProjectStatusString(prj.Status)

                                     }).Distinct().ToListAsync();

                return projects;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> UpdateProjectStatus(ProjectCorrentStatusDTO statusDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == statusDTO.ProjectId);
                    project.Status = (ProjectStatus)statusDTO.Status;

                    ProjectsStatus prodStatus = new ProjectsStatus
                    {
                        Id = new Guid(),
                        ProjectId = statusDTO.ProjectId,
                        SatusSetDate = DateTime.Now,
                        Comments = statusDTO.Comments,
                        Status = (ProjectStatus)statusDTO.Status,
                    };

                    _context.ProjectsStatus.Add(prodStatus);

                    int State = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return State == 1;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                } 
            }
        }

        public async Task<bool> CreateProject(ProjectDto dto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string ProjectNo = this.GetProjectNumber(dto);
                    string cnPid = Guid.NewGuid().ToString();

                    Project projectDomain = new Project
                    {
                        Id = cnPid,
                        Year = DateTime.Now.Year,
                        ProjectNo = ProjectNo,
                        Balance = dto.Budget,
                        ClientId = new Guid(dto.Client),
                        Week = dto.Week,
                        DeliveryDate = DateTime.Now.AddDays(dto.Week * 7),
                        StartDate = DateTime.Now,
                        Paid = 0,
                        Name = dto.Name,
                        Progress = 0,
                        Status = ProjectStatus.Budgeted,
                        CreatedDate = DateTime.Now,
                        Budget = dto.Budget,
                        Description =dto.Description
                    };

                    _context.Projects.Add(projectDomain);

                    this.AssignProjectEmploye(dto,cnPid);
                    
                    bool bStatus= await this.CreateProjectBudget(dto, cnPid, ProjectNo);

                    int State = await _context.SaveChangesAsync();
                   
                    await transaction.CommitAsync();
                    

                    return State == 1 ;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                } 
            }
        }

        private async Task<bool> CreateProjectBudget(ProjectDto dto, string cnPid, string projectNo)
        {
            try
            {
                ProjectApprovalDto dto1 = new ProjectApprovalDto
                {
                    ProjectNo = projectNo
                };
                string PmBudgetNo = GetPmBudgetNumber(dto1);

                var pmApproval = new ProjectBudgetActivities
                {
                    Id = Guid.NewGuid(),
                    BudgetNo = PmBudgetNo,
                    ProjectId = cnPid,
                    ProjectNo = dto.ProjectNo,
                    Budget = dto.Budget,
                    BudgetSubmitDate = DateTime.Now,
                    ApprovalSetUser = dto.SetUser
                };

                _context.ProjectBudgetActivities.Add(pmApproval);

                var Status = await _context.SaveChangesAsync();

                return Status >0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AssignProjectEmploye(ProjectDto dto,string cnPid)
        {
            foreach (ProjectEmp emp in dto.Engineers)
            {
                string pId = (Guid.NewGuid()).ToString();
                ProjectEmployee empP = new ProjectEmployee
                {
                    ProjectId = cnPid,
                    EmployeeId = emp.Id,
                    BudgetHours = emp.hour,
                    EmployeeType = EmployeeType.Engineering
                };

                _context.ProjectEmployees.Add(empP);
            }

            foreach (ProjectEmp emp in dto.Drawings)
            {
                string pId = (Guid.NewGuid()).ToString();
                ProjectEmployee empP = new ProjectEmployee
                {
                    ProjectId = cnPid,
                    EmployeeId = emp.Id,
                    BudgetHours = emp.hour,
                    EmployeeType = EmployeeType.Drawing
                };

                _context.ProjectEmployees.Add(empP);
            }
        }

        public string GetPmBudgetNumber(ProjectApprovalDto projectDto)
        {
            DateTime CurrentDate = DateTime.Now;
            var TodayPmCount = _context.ProjectBudgetActivities.Where(P => P.BudgetNo == projectDto.ProjectNo).Count() + 1;
            string PmBudgetNo = string.Format("{0}{1}", projectDto.ProjectNo, TodayPmCount.ToString("00"));

            return PmBudgetNo;
        }


    }
}
