using Application.DTOs;
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
                                  join cn in _context.Clients on em.ClientId equals cn.Id
                                  where ep.EmployeeId == empId
                                  select (new EpmProjectsDto
                                  {
                                      Id = em.Id,
                                      BHours = ep.BudgetHours,
                                      Client = cn.Name,
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
                param.ParameterName ="@WrkId";
                param.Value =new Guid( wrkId);
                cmd.Parameters.Add(param);

                List<HourslogDto> wrkList = new List<HourslogDto>();

                _context.Database.OpenConnection();
                using (DbDataReader rd = await cmd.ExecuteReaderAsync())
                {
                    while (rd.Read())
                    {

                        HourslogDto wOT = new HourslogDto()
                            {

                                WrkId = rd.GetValue("wrkId").ToString(),
                                WrkNo= rd.GetValue("WorkOrderNo").ToString() !=null? rd.GetValue("WorkOrderNo").ToString() :"",
                                WrkName = rd.GetValue("ConsWork").ToString() !=null ? rd.GetValue("ConsWork").ToString() :"",
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


        public  async Task<List<HourslogDto>> EmployeHourLogDetails(string wrkId)
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
    }
}
