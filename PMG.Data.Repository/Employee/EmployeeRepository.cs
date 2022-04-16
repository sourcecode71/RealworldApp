using Application.DTOs;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Employee
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _context;
        public EmployeeRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<List<EmployeeDto>> GetAllActiveEmployee()
        {
            try
            {
                var empList = await (from emp in _context.Employees
                                     join ur in _context.UserRoles on emp.Id equals ur.UserId
                                     join rl in _context.Roles on ur.RoleId equals rl.Id
                                     select new EmployeeDto
                                     {
                                         Id = emp.Id,
                                         Email = emp.Email,
                                         FirstName = emp.FirstName,
                                         LastName = emp.LastName,
                                         Name = string.Format("{0} {1}", emp.FirstName, emp.LastName),
                                         PhoneNumber = emp.PhoneNumber,
                                         Role = rl.Name
                                     }).ToListAsync();
                return empList;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EpmProjectsDto>> GetEmpProjects(string empId)
        {
            try
            {
                var pemp = await (from ep in _context.ProjectEmployees
                                  join em in _context.Projects on ep.ProjectId equals em.Id
                                  join cn in _context.Company on em.CompanyId equals cn.Id
                                  where ep.EmployeeId == empId
                                  select (new EpmProjectsDto
                                  {
                                      Id = em.Id,
                                      BHours = ep.BudgetHours,
                                      CompanyName = cn.Name,
                                      Name = em.Name,
                                      StartDateStr = em.StartDate.ToString("MM/dd/yyyy"),
                                      DeliveryDateStr = em.DeliveryDate.ToString("MM/dd/yyyy")
                                  })).ToListAsync();
                return pemp;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<HourslogDto>> EmployeHourLogSummery(string wrkId)
        {
            try
            {
                DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "dbo.Hrs_HourLogSummery";
                cmd.CommandType = CommandType.StoredProcedure;

                var param = cmd.CreateParameter();
                param.ParameterName = "@WrkId";
                param.Value = new Guid(wrkId);
                cmd.Parameters.Add(param);

                List<HourslogDto> wrkList = new List<HourslogDto>();

                _context.Database.OpenConnection();
                using (DbDataReader rd = await cmd.ExecuteReaderAsync())
                {
                    while (rd.Read())
                    {

                        HourslogDto wOT = new HourslogDto()
                        {

                            EmpId = rd.GetValue("empId").ToString(),
                            WrkId = rd.GetValue("wrkId").ToString(),
                            WrkNo = rd.GetValue("WorkOrderNo").ToString() != null ? rd.GetValue("WorkOrderNo").ToString() : "",
                            WrkName = rd.GetValue("ConsWork").ToString() != null ? rd.GetValue("ConsWork").ToString() : "",
                            Bhour = Convert.ToDouble(rd.GetValue("BudgetHours").ToString()),
                            Lhour = Convert.ToDouble(rd.GetValue("SpentHour").ToString()),
                            EmpType = rd.GetValue("RoleName").ToString(),
                            FirstName = rd.GetValue("FirstName") != null ? rd.GetValue("FirstName").ToString() : "",
                            LastName = rd.GetValue("LastName") != null ? rd.GetValue("LastName").ToString() : "",
                            IsActive = rd.GetValue("isDeleted") != null ? rd.GetValue("isDeleted").ToString() == "True" : false,
                        };

                        wOT.EmpName = String.Format("{0} {1}", wOT.FirstName, wOT.LastName);

                        wrkList.Add(wOT);

                    }
                }

                return wrkList;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<HourslogDto>> EmployeHourLogSummeryAll()
        {
            try
            {
                DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "dbo.Hrs_HourLogSummery_All";
                cmd.CommandType = CommandType.StoredProcedure;

                //var param = cmd.CreateParameter();
                //param.ParameterName = "@WrkId";
                //param.Value = new Guid(wrkId);
                //cmd.Parameters.Add(param);

                List<HourslogDto> wrkList = new List<HourslogDto>();

                _context.Database.OpenConnection();
                using (DbDataReader rd = await cmd.ExecuteReaderAsync())
                {
                    while (rd.Read())
                    {

                        HourslogDto wOT = new HourslogDto()
                        {

                            EmpId = rd.GetValue("empId").ToString(),
                            WrkId = rd.GetValue("wrkId").ToString(),
                            WrkNo = rd.GetValue("WorkOrderNo").ToString() != null ? rd.GetValue("WorkOrderNo").ToString() : "",
                            WrkName = rd.GetValue("ConsWork").ToString() != null ? rd.GetValue("ConsWork").ToString() : "",
                            Bhour = Convert.ToDouble(rd.GetValue("BudgetHours").ToString()),
                            Lhour = Convert.ToDouble(rd.GetValue("SpentHour").ToString()),
                            EmpType = rd.GetValue("RoleName").ToString(),
                            FirstName = rd.GetValue("FirstName") != null ? rd.GetValue("FirstName").ToString() : "",
                            LastName = rd.GetValue("LastName") != null ? rd.GetValue("LastName").ToString() : ""
                        };

                        wOT.EmpName = String.Format("{0} {1}", wOT.FirstName, wOT.LastName);

                        wrkList.Add(wOT);

                    }
                }

                return wrkList;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public async Task<List<HourslogDto>> EmployeHourLogDetails(string wrkId)
        {
            try
            {
                DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "dbo.Hrs_HourLogDetails";
                cmd.CommandType = CommandType.StoredProcedure;

                var param = cmd.CreateParameter();
                param.ParameterName = "@WrkId";
                param.Value = new Guid(wrkId);
                cmd.Parameters.Add(param);

                List<HourslogDto> hrsD = new List<HourslogDto>();

                _context.Database.OpenConnection();
                using (DbDataReader rd = await cmd.ExecuteReaderAsync())
                {
                    while (rd.Read())
                    {

                        HourslogDto wOT = new HourslogDto()
                        {
                            EmpId = rd.GetValue("EmpId").ToString() != null ? rd.GetValue("EmpId").ToString() : "",
                            WrkNo = rd.GetValue("WorkOrderNo").ToString() != null ? rd.GetValue("WorkOrderNo").ToString() : "",
                            WrkName = rd.GetValue("ConsWork").ToString() != null ? rd.GetValue("ConsWork").ToString() : "",
                            Lhour = Convert.ToDouble(rd.GetValue("SpentHour").ToString()),
                            EmpType = rd.GetValue("UserRole").ToString(),
                            FirstName = rd.GetValue("FirstName") != null ? rd.GetValue("FirstName").ToString() : "",
                            LastName = rd.GetValue("LastName") != null ? rd.GetValue("LastName").ToString() : "",
                            LogDateStr = rd.GetValue("SpentDate") != null ? rd.GetValue("SpentDate").ToString() : "",
                            ProjectNo = rd.GetValue("ProjectNo") != null ? rd.GetValue("ProjectNo").ToString() : ""

                        };

                        wOT.EmpName = String.Format("{0} {1}", wOT.FirstName, wOT.LastName);

                        hrsD.Add(wOT);

                    }
                }

                return hrsD;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<HourslogDto>> GetEmpWisehourLogs(string empId)
        {
            try
            {
                DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "dbo.Hrs_EmpWiseHourLogs";
                cmd.CommandType = CommandType.StoredProcedure;

                var param = cmd.CreateParameter();
                param.ParameterName = "@EmpId";
                param.Value = new Guid(empId);
                cmd.Parameters.Add(param);

                List<HourslogDto> hrsD = new List<HourslogDto>();

                _context.Database.OpenConnection();
                using (DbDataReader rd = await cmd.ExecuteReaderAsync())
                {
                    while (rd.Read())
                    {

                        HourslogDto wOT = new HourslogDto()
                        {
                            EmpId = rd.GetValue("EmpId").ToString() != null ? rd.GetValue("EmpId").ToString() : "",
                            WrkNo = rd.GetValue("WorkOrderNo").ToString() != null ? rd.GetValue("WorkOrderNo").ToString() : "",
                            WrkName = rd.GetValue("ConsWork").ToString() != null ? rd.GetValue("ConsWork").ToString() : "",
                            Lhour = Convert.ToDouble(rd.GetValue("SpentHour").ToString()),
                            EmpType = rd.GetValue("UserRole").ToString(),
                            FirstName = rd.GetValue("FirstName") != null ? rd.GetValue("FirstName").ToString() : "",
                            LastName = rd.GetValue("LastName") != null ? rd.GetValue("LastName").ToString() : "",
                            LogDateStr = rd.GetValue("SpentDate") != null ? rd.GetValue("SpentDate").ToString() : "",
                            ProjectNo = rd.GetValue("ProjectNo") != null ? rd.GetValue("ProjectNo").ToString() : "",
                            Remarks = rd.GetValue("Remarks") != null ? rd.GetValue("Remarks").ToString() : ""

                        };

                        wOT.EmpName = String.Format("{0} {1}", wOT.FirstName, wOT.LastName);

                        hrsD.Add(wOT);

                    }
                }

                return hrsD;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<EpmProjectsDto>> GetEmpWorkOrder(string empId)
        {
            try
            {
                var wemp = await (from ew in _context.WorkOrderEmployee
                                  join wr in _context.WorkOrder on ew.WorkOrderId equals wr.Id
                                  join p in _context.Projects on wr.ProjectId equals p.Id
                                  join em in _context.Employees on ew.EmployeeId equals em.Id
                                  join cm in _context.Company on p.CompanyId equals cm.Id into cmm
                                  from c in cmm.DefaultIfEmpty()
                                  where ew.EmployeeId == empId
                                  select (new EpmProjectsDto
                                  {
                                      Id = ew.Id.ToString(),
                                      BHours = ew.BudgetHours,
                                      CompanyName = c.Name,
                                      Name = string.Format("{0} {1}", em.FirstName, em.LastName),
                                      StartDateStr = wr.StartDate.ToString("MM/dd/yyyy"),
                                      DeliveryDateStr = wr.EndDate.ToString("MM/dd/yyyy"),
                                      WrkNo = wr.WorkOrderNo,
                                      Year = p.Year,
                                      ProjectNo = p.ProjectNo,
                                      ConsWork = wr.ConsWork

                                  })).ToListAsync();
                return wemp;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<HourslogDto>> GetEmpWiseWrkOThourLogs(string empId, string wrkId)
        {
            try
            {
                DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "dbo.Hrs_EmpWiseWrkHourLogs";
                cmd.CommandType = CommandType.StoredProcedure;


                var param = cmd.CreateParameter();
                param.ParameterName = "@EmpId";
                param.Value = new Guid(empId);
                cmd.Parameters.Add(param);

                var paramWrk = cmd.CreateParameter();
                paramWrk.ParameterName = "@WrkId";
                paramWrk.Value = new Guid(wrkId);
                cmd.Parameters.Add(paramWrk);



                List<HourslogDto> hrsD = new List<HourslogDto>();

                _context.Database.OpenConnection();
                using (DbDataReader rd = await cmd.ExecuteReaderAsync())
                {
                    while (rd.Read())
                    {

                        HourslogDto wOT = new HourslogDto()
                        {
                            EmpId = rd.GetValue("EmpId").ToString() != null ? rd.GetValue("EmpId").ToString() : "",
                            WrkNo = rd.GetValue("WorkOrderNo").ToString() != null ? rd.GetValue("WorkOrderNo").ToString() : "",
                            WrkName = rd.GetValue("ConsWork").ToString() != null ? rd.GetValue("ConsWork").ToString() : "",
                            Lhour = Convert.ToDouble(rd.GetValue("SpentHour").ToString()),
                            EmpType = rd.GetValue("UserRole").ToString(),
                            FirstName = rd.GetValue("FirstName") != null ? rd.GetValue("FirstName").ToString() : "",
                            LastName = rd.GetValue("LastName") != null ? rd.GetValue("LastName").ToString() : "",
                            LogDateStr = rd.GetValue("SpentDate") != null ? rd.GetValue("SpentDate").ToString() : "",
                            ProjectNo = rd.GetValue("ProjectNo") != null ? rd.GetValue("ProjectNo").ToString() : "",
                            Remarks = rd.GetValue("Remarks") != null ? rd.GetValue("Remarks").ToString() : ""

                        };

                        wOT.EmpName = String.Format("{0} {1}", wOT.FirstName, wOT.LastName);

                        hrsD.Add(wOT);

                    }
                }

                return hrsD;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> UpdateEmaployeeAssignState(HourslogDto dto)
        {
            try
            {
                var empassign = _context.WorkOrderEmployee.FirstOrDefault(e => e.EmployeeId == dto.EmpId && e.WorkOrderId == new Guid(dto.WrkId));
                // empassign.OriginalBHours = empassign.BudgetHours;
                if (empassign == null)
                {
                    return false;
                }
                else
                {
                    empassign.IsDeleted = dto.IsActive;
                    await _context.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> SetupEmployeeForWrk(HourslogDto dto)
        {
            try
            {
                var exitHrs = _context.WorkOrderEmployee.Where(e => e.EmployeeId == dto.EmpId && e.WorkOrderId == new Guid(dto.WrkId)).FirstOrDefault();
                if (exitHrs == null)
                {
                    WorkOrderEmployee workOrderEmp = new WorkOrderEmployee
                    {
                        EmployeeId = dto.EmpId,
                        WorkOrderId = new Guid(dto.WrkId),
                        BudgetHours = dto.Bhour,
                        OriginalBHours = dto.Bhour
                    };

                    _context.WorkOrderEmployee.Add(workOrderEmp);
                }
                else
                {
                    exitHrs.BudgetHours = dto.Bhour;
                    exitHrs.OriginalBHours = dto.Bhour;
                }

                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EmployeeDto> GetEmployeeById(string empId)
        {
            try
            {
                var empList = await (from emp in _context.Employees
                                     join ur in _context.UserRoles on emp.Id equals ur.UserId
                                     join rl in _context.Roles on ur.RoleId equals rl.Id
                                     where (emp.Id == empId)
                                     select new EmployeeDto
                                     {
                                         Id = emp.Id,
                                         Email = emp.Email,
                                         FirstName = emp.FirstName,
                                         LastName = emp.LastName,
                                         Name = string.Format("{0} {1}", emp.FirstName, emp.LastName),
                                         PhoneNumber = emp.PhoneNumber,
                                         Role = rl.Name
                                     }).FirstOrDefaultAsync();
                return empList;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

       
    }

}

