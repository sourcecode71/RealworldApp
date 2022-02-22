using Application.DTOs;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Projects
{
    public class Projects : IProjects
    {
        private readonly DataContext _context;
        public Projects(DataContext dataContext)
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
            var projects = await _context.Projects.Where(p => p.Name.Contains(SearchTag)).Take(20).Select(p=> new
            ProjectDto()
            {
                Id = p.Id,
                Name = p.Name,
                Client=p.Client,
                Budget=p.Budget,
                Balance =p.Balance,
                DeliveryDate=p.DeliveryDate,
                Description =p.Description,
                Year = p.Year,
                ProjectNo = p.ProjectNo
            }).ToListAsync();
           
            return projects;
        }

        public string GetPmBudgetNumber(ProjectApprovalDto projectDto)
        {
            DateTime CurrentDate = DateTime.Now;
            var TodayPmCount = _context.ProjectBudgetActivities.Where(P => P.BudgetNo == projectDto.ProjectNo).Count() + 1;
            string PmBudgetNo = string.Format("{0}{1}", projectDto.ProjectNo,TodayPmCount.ToString("00"));

            return PmBudgetNo;
        }

        public async Task<bool> SaveProjectApproval(ProjectApprovalDto approvalDto)
        {
            try
            {
                string PmBudgetNo = GetPmBudgetNumber(approvalDto);

                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == approvalDto.Id);

                if(project == null)
                {
                    return false;
                }

                project.IsBudgetApproved = approvalDto.ApporvalSatus == 1;

                decimal budgetBalance = (decimal)project.Balance;


                if (approvalDto.ApporvalSatus == 1 && approvalDto.ApprovedBudget>0)
                {
                    project.Balance = project.Balance - (double)approvalDto.ApprovedBudget;

                    budgetBalance = (decimal)project.Balance;
                }


                var pmApproval = new ProjectBudgetActivities
                { 
                     Id  = Guid.NewGuid(),
                     BudgetNo = PmBudgetNo,
                     ProjectId = approvalDto.Id,
                     ProjectNo = approvalDto.ProjectNo,
                     ApprovalStatus = approvalDto.ApporvalSatus ==1,
                     ApprovedBudget = approvalDto.ApprovedBudget,
                     BalanceBudget = budgetBalance,
                     Comments = approvalDto.Comments,
                     ApprovedDate = DateTime.Now,
                };

                _context.Add(pmApproval);

                var Status = await _context.SaveChangesAsync();

                return Status == 1 ;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<ProjectApprovalDto>> LoadProjectBudgetAcitivies(string projectName)
        {
            try
            {
                var prjoect = (from pa in _context.ProjectBudgetActivities
                               join pj in _context.Projects on pa.ProjectId equals pj.Id
                               orderby pa.SetDate descending
                               select new ProjectApprovalDto
                               {
                                   Id = pj.Id,
                                   ProjectNo = pj.ProjectNo,
                                   BudegtNo = pa.BudgetNo,
                                   ApprovedBudget = pa.ApprovedBudget,
                                   ClientName = pj.Client,
                                   ApprovalStatus = pa.ApprovalStatus,
                                   ApprovalDateStr = pa.ApprovedDate.ToString("dd/MM/yyyy"),
                                   ProjectName = pj.Name,
                                   Balance = pj.Balance,
                                   ProjectId = pa.Id

                               }).Take(50).ToListAsync();

                return await prjoect;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
