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
        Task<List<HourlogsDTO>> GetAllHourLogs(string empId,EmployeeType empType);
        Task<List<HourlogsDTO>> GetWorkOrderHourLogs(string empId, string wrkId, EmployeeType empType);

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
                   SetDate=DateTime.Now,
                   SetUser = dTO.SetUser,

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
                    Phone=dTO.Phone,
                    SetUser=dTO.SetUser,
                    SetDate=DateTime.Now
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
                    var prevHour = _context.Hourlogs.Where(p=>p.WorkOrderId == dTO.WorkOrderId && p.EmpId==dTO.EmpId).Sum(ep=>ep.SpentHour);

                    Hourlogs hourlogs = new Hourlogs
                    {
                        Id = new Guid(),
                        EmpId = dTO.EmpId,
                        WorkOrderId = dTO.WorkOrderId,
                        SpentHour = dTO.SpentHour,
                        SpentDate = dTO.SpentDate,
                        BalanceHour = (prevHour + dTO.SpentHour),
                        Remarks = dTO.Remarks,
                        SetDate= DateTime.Now,
                        HourLogFor =1,
                        SetUser = dTO.SetUser
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

        public async Task<List<HourlogsDTO>> GetAllHourLogs(string empId, EmployeeType empType)
        {
            try
            {
                var hrLogs = (from hr in _context.Hourlogs
                                    join wrk in _context.WorkOrder on hr.WorkOrderId equals wrk.Id 
                                    join e in _context.Employees on hr.EmpId.ToString() equals e.Id
                                    join p in _context.Projects on wrk.ProjectId equals p.Id
                                    where (wrk.Status !=ProjectStatus.Archived)
                                    select new HourlogsDTO
                                    {
                                        EmpId = hr.EmpId,
                                        EmpName = String.Format("{0} {1}", e.FirstName,e.LastName),
                                        SpentHour = hr.SpentHour,
                                        SpentDate = hr.SpentDate,
                                        BalanceHour = hr.BalanceHour,
                                        WorkOrderId = hr.WorkOrderId,
                                        WorkOrderNo= wrk.WorkOrderNo,
                                        WorkOrderName = wrk.ConsWork,
                                        ProjectNo = p.ProjectNo,
                                        ProjectId = p.Id,
                                        ProjectName = p.Name,
                                        Year = p.Year,
                                        SpentDateStr = hr.SpentDate.ToString("MM/dd/yyyy"),
                                        Remarks = hr.Remarks
                                    }).AsQueryable();

                       if(empType == EmployeeType.Engineering || empType == EmployeeType.Drawing)
                        {

                           var result = await hrLogs.Where(p=>p.EmpId == new Guid(empId) ).ToListAsync();
                           return result;
                        }
                        else
                        {
                            var result = await hrLogs.ToListAsync();
                            return result;
                        }
                       
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public async Task<List<HourlogsDTO>> GetWorkOrderHourLogs(string empId,string wrkId, EmployeeType empType)
        {
            try
            {
                var hrLogs = (from hr in _context.Hourlogs
                              join wrk in _context.WorkOrder on hr.WorkOrderId equals wrk.Id
                              join e in _context.Employees on hr.EmpId.ToString() equals e.Id
                              join p in _context.Projects on wrk.ProjectId equals p.Id
                              where (wrk.Status != ProjectStatus.Archived && wrk.Id == new Guid(wrkId))
                              select new HourlogsDTO
                              {
                                  EmpId = hr.EmpId,
                                  EmpName = String.Format("{0} {1}", e.FirstName, e.LastName),
                                  SpentHour = hr.SpentHour,
                                  SpentDate = hr.SpentDate,
                                  BalanceHour = hr.BalanceHour,
                                  WorkOrderId = hr.WorkOrderId,
                                  WorkOrderNo = wrk.WorkOrderNo,
                                  WorkOrderName = wrk.ConsWork,
                                  ProjectNo = p.ProjectNo,
                                  ProjectId = p.Id,
                                  ProjectName = p.Name,
                                  Year = p.Year,
                                  SpentDateStr = hr.SpentDate.ToString("MM/dd/yyyy"),
                                  Remarks = hr.Remarks
                              }).AsQueryable();

                if (empType == EmployeeType.Engineering || empType == EmployeeType.Drawing)
                {

                    var result = await hrLogs.Where(p => p.EmpId == new Guid(empId)).ToListAsync();
                    return result;
                }
                else
                {
                    var result = await hrLogs.ToListAsync();
                    return result;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
