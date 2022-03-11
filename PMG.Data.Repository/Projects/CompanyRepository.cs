using Application.DTOs;
using Domain.Common;
using Domain.Enums;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Projects
{
    public interface ICompanyRepository 
    {
        Task<bool> SaveCleint(ClientDTO dTO);
        Task<bool> SaveCompany(CompanyDTO dTO);
        IQueryable<Client> GetAllClient();
        Task <List<CompanyDTO>> GetAllCompany();
        Task<List<CompanyDTO>> GetAllCompany(Guid guid);
        Task<bool> SaveEmployeHourLog(HourlogsDTO dTO);
        Task<List<HourlogsDTO>> GetAllHourLogs(string empId,string empType);

    }
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DataContext _context;
        public CompanyRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public IQueryable<Client> GetAllClient()
        {
            return _context.Clients.Where(p => p.IsActive).OrderBy(p=>p.Name);
        }

        public async Task<List<CompanyDTO>> GetAllCompany()
        {
            try
            {
                var company = await (from cm in _context.Company
                                     join cl in _context.Clients on cm.ClientId equals cl.Id
                                     where (cm.IsActive && cl.IsActive)
                                     orderby cm.Name 
                                     select new CompanyDTO
                                     {
                                         Id = cm.Id,
                                         Address = cm.Address,
                                         Name = cm.Name,
                                         ClientName = cl.Name,
                                         Email = cm.Email,
                                         Phone = cm.Phone,
                                         ContactName = cm.ContactName,

                                     }).ToListAsync();

                return company;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<List<CompanyDTO>> GetAllCompany(Guid guid)
        {
            try
            {
                var company = await (from cm in _context.Company
                                     join cl in _context.Clients on cm.ClientId equals cl.Id
                                     where (cm.IsActive && cl.IsActive && cm.ClientId == guid)
                                     select new CompanyDTO
                                     {
                                         Id = cm.Id,
                                         Address = cm.Address,
                                         Name = cm.Name,
                                         ClientName = cl.Name
                                     }).ToListAsync();

                return company;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

     

        public async Task<bool> SaveCleint(ClientDTO dTO)
        {
            try
            {
               Client  client = new Client
               {
                   Name =dTO.Name,
                   Address =dTO.Address,
                   ContactName = dTO.ContactName,
                   email = dTO.Email,
                   phone= dTO.Phone,
               };
                _context.Clients.Add(client);
                int State = await _context.SaveChangesAsync();

                return State == 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveCompany(CompanyDTO dTO)
        {
            try
            {
                Company company = new Company
                {
                    Name = dTO.Name,
                    Address = dTO.Address,
                    ClientId = dTO.ClientId,
                    ContactName =dTO.ContactName,
                    Email = dTO.Email,
                    Phone=dTO.Phone
                };

                _context.Company.Add(company);
                int State = await _context.SaveChangesAsync();
                return State ==1 ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveEmployeHourLog(HourlogsDTO dTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var budgetProject = await _context.ProjectEmployees.FirstOrDefaultAsync(p => p.ProjectId == dTO.ProjectId.ToString() && p.EmployeeId == dTO.EmpId.ToString());
                    if (budgetProject == null)
                    {
                        return false;
                    }

                    budgetProject.TotalHourLog = budgetProject.TotalHourLog + dTO.SpentHour;

                    Hourlogs hourlogs = new Hourlogs
                    {
                        Id = new Guid(),
                        EmpId = dTO.EmpId,
                        ProjectId = dTO.ProjectId,
                        SpentHour = dTO.SpentHour,
                        SpentDate = dTO.SpentDate,
                        BalanceHour = (budgetProject.BudgetHours - dTO.SpentHour),
                        Remarks = dTO.Remarks,
                        SetDate=dTO.SpentDate,
                        SetUser = dTO.EmpId.ToString()
                    };

                    _context.Hourlogs.Add(hourlogs);

                    var state = await _context.SaveChangesAsync();
                    transaction.Commit();
                    return state == 1;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<List<HourlogsDTO>> GetAllHourLogs(string empId, string empType)
        {
            try
            {
                var hrLogs = await (from hr in _context.Hourlogs
                                    join p in _context.Projects on hr.ProjectId.ToString() equals p.Id
                                    join e in _context.Employees on hr.EmpId.ToString() equals e.Id
                                    where (p.Status != ProjectStatus.Completed)
                                    select new HourlogsDTO
                                    {
                                        EmpName = e.Name,
                                        SpentHour = hr.SpentHour,
                                        SpentDate = hr.SpentDate,
                                        Project = p.Name,
                                        ProjectNo = p.ProjectNo,
                                        SpentDateStr = hr.SetDate.ToString("MM/dd/yyyy"),
                                        Remarks = hr.Remarks
                                    }).ToListAsync();

                         return hrLogs;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
