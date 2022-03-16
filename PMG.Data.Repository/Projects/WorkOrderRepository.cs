using Application.DTOs;
using Domain.Enums;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Projects
{
    public class WorkOrderRepository : IWorkOrderRepository
    {
        private readonly DataContext _context;
        public WorkOrderRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public List<WorkOrderDTO> LoadAllWorkOrders()
        {
            try
            {
                DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "dbo.Wrk_AllWorkOrder";
                cmd.CommandType = CommandType.StoredProcedure;

                List<WorkOrderDTO> wrkList = new List<WorkOrderDTO>();

                _context.Database.OpenConnection();
                using (DbDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        WorkOrderDTO wOT = new WorkOrderDTO()
                        {
                            Id = new Guid(rd.GetValue("id").ToString()),
                            ProjectId = rd.GetValue("ProjectId").ToString(),
                            ClinetName = rd.GetValue("ClientName").ToString(),
                            ProjectName = rd.GetValue("ProjectName").ToString(),
                            OriginalBudget =Convert.ToDouble( rd.GetValue("OriginalBudget").ToString()),
                            ApprovedBudget =Convert.ToDouble( rd.GetValue("ApprovedBudget").ToString()),
                            Balance = Convert.ToDouble( rd.GetValue("Balance").ToString()),
                            SpentHour = Convert.ToDouble( rd.GetValue("SpentHour").ToString()),
                            ConsecutiveWork = rd.GetValue("ConsWork").ToString(),
                            OTDescription = rd.GetValue("OTDescription").ToString(),
                            CompanyName = rd.GetValue("CompanyName") != null? rd.GetValue("CompanyName").ToString() :"",
                            ClientName = rd.GetValue("ClientName") != null ? rd.GetValue("ClientName").ToString() : "",
                            ProjectNo = rd.GetValue("ProjectNo").ToString(),
                            ProjectYear =Convert.ToInt16( rd.GetValue("pYear").ToString()),
                            WorkOrderNo = rd.GetValue("WorkOrderNo").ToString(),
                            WrkBudgetNo = rd.GetValue("BudgetNo").ToString(),
                            WrkStatus =(ProjectStatus)(Convert.ToInt16 (rd.GetValue("WrkStatus").ToString())),
                            StartDateStr = rd.GetValue("SubmitDate").ToString(),
                            EndDateStr = rd.GetValue("EnDate").ToString(),
                            ApprovedDateStr = rd.GetValue("ApprovalDate").ToString(),

                        };

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

        public async Task<bool> SaveWorkOrder(WorkOrderDTO dTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string OTNo = GetWorkOrderNumber(dTO);
                    Guid guid = Guid.NewGuid();

                    var pmWorkOrder = new WorkOrder
                    {
                        Id = guid,
                        WorkOrderNo = OTNo,
                        ConsWork = dTO.ConsecutiveWork,
                        ProjectId = dTO.ProjectId,
                        CompanyId = new Guid(dTO.CompanyId),
                        OriginalBudget = dTO.OriginalBudget,
                        Balance = dTO.OriginalBudget,
                        StartDate = dTO.StartDate,
                        EndDate = dTO.EndDate,
                        OTDescription = dTO.OTDescription,
                        BudgetStatus = 0
                    };

                    _context.WorkOrder.Add(pmWorkOrder);

                   this.AssignWorkOrderEmploye(dTO, guid.ToString());

                   bool State= await this.CreateWorkOrderBudget(dTO, guid, OTNo);

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

        public async Task<bool> UpdateWorkOrder(WorkOrderDTO dTO)
        {

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                    WorkOrder workOrder = await _context.WorkOrder.FirstOrDefaultAsync(p => p.Id == new Guid(dTO.WorkOrderId) );

                    HisWorkOrder hisWork = new HisWorkOrder
                        {
                            Id = Guid.NewGuid(),
                            ApprovalDate = workOrder.ApprovalDate,
                            ApprovedBudget = workOrder.ApprovedBudget,
                            Comments = workOrder.Comments,
                            IsDeleted = workOrder.IsDeleted,
                            OTDescription = workOrder.OTDescription,
                            ProjectId = workOrder.ProjectId,
                            ProjectNo = workOrder.ProjectNo,
                            SetDate = workOrder.SetDate,
                            SetUser = workOrder.SetUser,
                            WorkOrderId = workOrder.Id,
                            WorkOrderNo = workOrder.WorkOrderNo,
                        };

                        _context.HisWorkOrder.Add(hisWork);

                        workOrder.UpdateDate = DateTime.Now;
                        workOrder.UpdateUser = "admin";
                        workOrder.Comments = dTO.Comments;
                        workOrder.ApprovedBudget = dTO.ApprovedBudget;
                        workOrder.Balance = dTO.ApprovedBudget;
                        workOrder.ProjectId = dTO.ProjectId;
                        workOrder.ProjectNo = dTO.ProjectNo;

                       int state = await _context.SaveChangesAsync();
                       await transaction.CommitAsync();

                        return state == 1;

                    }
                    catch (Exception ex)
                    {
                       await transaction.RollbackAsync();
                        throw ex;
                    }
                }
        }


        public IQueryable<WorkOrderDTO> GetFilteredWorkOrder(string strOT)
        {
            try
            {
                var wrkODT = from wrk in _context.WorkOrder
                             join prj in _context.Projects on wrk.ProjectId equals prj.Id
                             where wrk.OTDescription.Contains(strOT) 
                             orderby wrk.ApprovalDate descending
                             select new WorkOrderDTO
                             {
                                 Id = wrk.Id,
                                 ApprovedBudget = wrk.ApprovedBudget,
                                 OTDescription = wrk.OTDescription,
                                 ApprovedDate = wrk.ApprovalDate,
                                 ProjectId = wrk.ProjectId,
                                 ProjectNo = wrk.ProjectNo,
                                 WorkOrderNo = wrk.WorkOrderNo,
                                 ProjectName = prj.Name,
                                 ClinetName = prj.Name,
                                 ProjectYear = prj.Year,
                                 Comments = wrk.Comments
                             };
                return wrkODT;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



       private async Task<bool> CreateWorkOrderBudget(WorkOrderDTO dto, Guid wrkId, string wrkNo)
        {
            try
            {
               
                string wrkBudgetNo = GetWrkNumber(wrkNo);

                var wrkApproval = new WorkOrderActivities
                {
                    Id = Guid.NewGuid(),
                    BudgetNo = wrkBudgetNo,
                    WorkOrderId = wrkId,
                    WorkOrderNo = wrkNo,
                    Budget = dto.OriginalBudget,
                    BudgetSubmitDate = DateTime.Now,
                    ApprovalSetUser = dto.SetUser
                };

                _context.WorkOrderActivities.Add(wrkApproval);

                var Status = await _context.SaveChangesAsync();

                return Status > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       private void AssignWorkOrderEmploye(WorkOrderDTO dto, string wrkId)
        {
            foreach (ProjectEmp emp in dto.Engineers)
            {
                string pId = (Guid.NewGuid()).ToString();
                WorkOrderEmployee empW = new WorkOrderEmployee
                {
                    WorkOrderId =new Guid(wrkId),
                    EmployeeId = emp.Id,
                    BudgetHours = emp.hour,
                    EmployeeType = EmployeeType.Engineering
                };

                _context.WorkOrderEmployee.Add(empW);
            }

            foreach (ProjectEmp emp in dto.Drawings)
            {
                string pId = (Guid.NewGuid()).ToString();
                WorkOrderEmployee empWD = new WorkOrderEmployee
                {
                    WorkOrderId = new Guid(wrkId),
                    EmployeeId = emp.Id,
                    BudgetHours = emp.hour,
                    EmployeeType = EmployeeType.Drawing
                };

                _context.WorkOrderEmployee.Add(empWD);
            }
        }

        private string GetWorkOrderNumber(WorkOrderDTO dTO)
        {
            DateTime CurrentDate = DateTime.Now;

            string Day = CurrentDate.Day.ToString("00");
            string Month = CurrentDate.Month.ToString("00");
            string Year = CurrentDate.Year.ToString();
            string ProjectNumber = string.Format("{0}{1}{2}", Day, Month, Year);

            var PmOTCount = _context.WorkOrder.Where(P => P.ProjectNo == dTO.ProjectNo).Count() + 1;
            
            string workOrderNo = string.Format("{0}{1}{2}", ProjectNumber, "OT", PmOTCount.ToString("00"));
            return workOrderNo;
        }

        public string GetWrkNumber(string wrkNo)
        {
            DateTime CurrentDate = DateTime.Now;
            var TodayWrkCount = _context.WorkOrderActivities.Where(P => P.WorkOrderNo == wrkNo).Count() + 1;
            string wrkBudgetNo = string.Format("{0}{1}", wrkNo, TodayWrkCount.ToString("00"));

            return wrkBudgetNo;
        }

        public async Task<bool> UpdateWorkOrderStatus(WorkOrderDTO dTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    WorkOrder workOrder = await _context.WorkOrder.FirstOrDefaultAsync(p => p.Id == new Guid(dTO.WorkOrderId));

                    HisWorkOrder hisWork = new HisWorkOrder
                    {
                        Id = Guid.NewGuid(),
                        ApprovalDate = workOrder.ApprovalDate,
                        ApprovedBudget = workOrder.ApprovedBudget,
                        Comments = dTO.Comments,
                        IsDeleted = workOrder.IsDeleted,
                        OTDescription = workOrder.OTDescription,
                        ProjectId = workOrder.ProjectId,
                        ProjectNo = workOrder.ProjectNo,
                        SetDate = workOrder.SetDate,
                        SetUser = workOrder.SetUser,
                        WorkOrderId = workOrder.Id,
                        WorkOrderNo = workOrder.WorkOrderNo,
                        ChangeFor = 3,
                    };

                    _context.HisWorkOrder.Add(hisWork);

                    workOrder.UpdateDate = DateTime.Now;
                    workOrder.UpdateUser = "admin";
                    workOrder.Status = (ProjectStatus)dTO.Status;

                    int state = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return state == 1;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }

        public async Task<List<WorkOrderDTO>> LoadAllWorkOrdersByEmp(string EmpId)
        {
            try
            {
                List<WorkOrderDTO> wrkList = await (from w in _context.WorkOrder
                                                    join p in _context.Projects on w.ProjectId equals p.Id
                                                    join ew in _context.WorkOrderEmployee on w.Id  equals ew.WorkOrderId 
                                                    where ew.EmployeeId == EmpId
                                                    orderby w.SetDate descending
                                                    select new WorkOrderDTO
                                                    {
                                                        Id = w.Id,
                                                        ProjectId = w.ProjectId,
                                                        ProjectName = p.Name,
                                                        OriginalBudget = w.OriginalBudget,
                                                        ApprovedBudget = w.ApprovedBudget,
                                                        ConsecutiveWork = w.ConsWork,
                                                        Comments = w.Comments,
                                                        OTDescription = w.OTDescription,
                                                        ProjectNo = w.ProjectNo,
                                                        ProjectYear = p.Year,
                                                        WorkOrderNo = w.WorkOrderNo,
                                                        ProjectBudget = p.Budget,
                                                        StartDateStr = w.StartDate.ToString("MM/dd/yyyy"),
                                                        EndDateStr = w.EndDate.ToString("MM/dd/yyyy"),
                                                        ApprovedDateStr = w.ApprovalDate.ToString("MM/dd/yyyy")

                                                    }).ToListAsync();
           

                return wrkList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<WorkOrderDTO>> WorkOrderByProjects(string PrId)
        {
            try
            {
                var wrkDT = await (from p in _context.Projects
                             join w in _context.WorkOrder on p.Id equals w.ProjectId
                             where p.Id == PrId
                             select (new WorkOrderDTO
                             {
                                 Id = w.Id,
                                 ProjectNo = p.ProjectNo,
                                 ProjectName = p.Name,
                                 ProjectYear = p.Year,
                                 WorkOrderNo = w.WorkOrderNo,
                                 ConsecutiveWork = w.ConsWork,
                                 ApprovedBudget = w.ApprovedBudget,
                                 StartDateStr = w.StartDate.ToString("MM/dd/yyyy"),
                                 EndDateStr = w.EndDate.ToString("MM/dd/yyyy"),
                                 OriginalBudget = w.OriginalBudget
                             })).ToListAsync();
                return wrkDT;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }


 
}
