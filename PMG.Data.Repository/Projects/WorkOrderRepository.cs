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
                            OriginalBudget = Convert.ToDouble(rd.GetValue("OriginalBudget").ToString()),
                            ApprovedBudget = Convert.ToDouble(rd.GetValue("ApprovedBudget").ToString()),
                            Balance = Convert.ToDouble(rd.GetValue("Balance").ToString()),
                            BudgetHour = Convert.ToDouble(rd.GetValue("BudgetHours").ToString()),
                            SpentHour = Convert.ToDouble(rd.GetValue("SpentHour").ToString()),
                            ConsecutiveWork = rd.GetValue("ConsWork").ToString(),
                            OTDescription = rd.GetValue("OTDescription").ToString(),
                            CompanyName = rd.GetValue("CompanyName") != null ? rd.GetValue("CompanyName").ToString() : "",
                            ClientName = rd.GetValue("ClientName") != null ? rd.GetValue("ClientName").ToString() : "",
                            ProjectNo = rd.GetValue("ProjectNo").ToString(),
                            ProjectYear = Convert.ToInt16(rd.GetValue("pYear").ToString()),
                            WorkOrderNo = rd.GetValue("WorkOrderNo").ToString(),
                            WrkBudgetNo = rd.GetValue("BudgetNo").ToString(),
                            WrkStatus = (ProjectStatus)(Convert.ToInt16(rd.GetValue("WrkStatus").ToString())),
                            StartDateStr = rd.GetValue("StartDate").ToString(),
                            EndDateStr = rd.GetValue("EnDate").ToString(),
                            ApprovedDateStr = rd.GetValue("ApprovalDate").ToString(),

                        };

                        wOT.WrkStatusStr = this.GetStatusString(wOT.WrkStatus);


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


        public async Task<WorkOrderDTO> LoadWorkOrdersById(string wrkId)
        {
            try
            {
                DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "dbo.Wrk_WorkOrderById";
                cmd.CommandType = CommandType.StoredProcedure;

                var param = cmd.CreateParameter();
                param.ParameterName = "@WrkId";
                param.Value = new Guid(wrkId);
                cmd.Parameters.Add(param);

                WorkOrderDTO wOTNull = new WorkOrderDTO()
                {

                };
                _context.Database.OpenConnection();
                using (DbDataReader rd = await cmd.ExecuteReaderAsync())
                {
                    while (rd.Read())
                    {
                        WorkOrderDTO wOT = new WorkOrderDTO()
                        {
                            Id = new Guid(rd.GetValue("id").ToString()),
                            ProjectId = rd.GetValue("ProjectId").ToString(),
                            ClinetName = rd.GetValue("ClientName").ToString(),
                            ProjectName = rd.GetValue("ProjectName").ToString(),
                            OriginalBudget = Convert.ToDouble(rd.GetValue("OriginalBudget").ToString()),
                            ApprovedBudget = Convert.ToDouble(rd.GetValue("ApprovedBudget").ToString()),
                            Balance = Convert.ToDouble(rd.GetValue("Balance").ToString()),
                            SpentHour = Convert.ToDouble(rd.GetValue("SpentHour").ToString()),
                            ConsecutiveWork = rd.GetValue("ConsWork").ToString(),
                            OTDescription = rd.GetValue("OTDescription").ToString(),
                            CompanyName = rd.GetValue("CompanyName") != null ? rd.GetValue("CompanyName").ToString() : "",
                            ClientName = rd.GetValue("ClientName") != null ? rd.GetValue("ClientName").ToString() : "",
                            ProjectNo = rd.GetValue("ProjectNo").ToString(),
                            ProjectYear = Convert.ToInt16(rd.GetValue("pYear").ToString()),
                            WorkOrderNo = rd.GetValue("WorkOrderNo").ToString(),
                            WrkBudgetNo = rd.GetValue("BudgetNo").ToString(),
                            WrkStatus = (ProjectStatus)(Convert.ToInt16(rd.GetValue("WrkStatus").ToString())),
                            StartDateStr = rd.GetValue("SubmitDate").ToString(),
                            EndDateStr = rd.GetValue("EnDate").ToString(),
                            ApprovedDateStr = rd.GetValue("ApprovalDate").ToString(),

                        };

                        wOT.WrkStatusStr = this.GetStatusString(wOT.WrkStatus);

                        return wOT;

                    }
                }

                return wOTNull;
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
                        Year = dTO.StartDate.Year,
                        ConsWork = dTO.ConsecutiveWork,
                        ProjectId = dTO.ProjectId,
                        OriginalBudget = dTO.OriginalBudget,
                        StartDate = dTO.StartDate,
                        EndDate = dTO.EndDate,
                        OTDescription = dTO.OTDescription,
                        BudgetStatus = 0
                    };

                    _context.WorkOrder.Add(pmWorkOrder);

                    this.AssignWorkOrderEmploye(dTO, guid.ToString());

                    bool State = await this.CreateWorkOrderBudget(dTO, guid, OTNo);

                    var Status = await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return Status > 0;
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
                    WorkOrder workOrder = await _context.WorkOrder.FirstOrDefaultAsync(p => p.Id == new Guid(dTO.WorkOrderId));

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

                string wrkBudgetNo = GetWrkNumber(wrkNo, wrkId);
                string budgetVersion = GetBudgetVersion(wrkNo, wrkId);

                var wrkApproval = new WorkOrderActivities
                {
                    Id = Guid.NewGuid(),
                    BudgetNo = wrkBudgetNo,
                    BudgetVersionNo = budgetVersion,
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
                    WorkOrderId = new Guid(wrkId),
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

            string Year = CurrentDate.Year.ToString();
            var PmOTCount = _context.WorkOrder.Where(P => P.Year == dTO.StartDate.Year).Count() + 1;
            string workOrderNo = string.Format("{0}{1}", Year, PmOTCount.ToString("00000"));
            return workOrderNo;
        }

        public string GetWrkNumber(string wrkNo, Guid wrkId)
        {
            DateTime CurrentDate = DateTime.Now;
            var TodayWrkCount = _context.HisBudgetActivities.Where(P => P.WorkOrderId == wrkId).Count() + 1;
            string wrkBudgetNo = string.Format("{0}{1}", wrkNo, TodayWrkCount.ToString("00"));

            return wrkBudgetNo;
        }

        private string GetBudgetVersion(string wrkNo, Guid wrkId)
        {
            int vCount = _context.HisBudgetActivities.Where(P => P.WorkOrderId == wrkId).Count() + 1;
            string versionNO = string.Format("v{0}", vCount.ToString("00"));
            return versionNO;
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
                                                    join ew in _context.WorkOrderEmployee on w.Id equals ew.WorkOrderId
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

        private string GetStatusString(ProjectStatus status)
        {
            switch (status)
            {
                case ProjectStatus.Budgeted:
                    return "Budget phase";
                case ProjectStatus.Active:
                    return "Active";
                case ProjectStatus.Completed:
                    return "Completed";
                case ProjectStatus.Archived:
                    return "Archived";
                case ProjectStatus.Delayed:
                    return "Delayed";
                case ProjectStatus.Canceled:
                    return "Cancel";
                default:
                    return "On Time";
            }
        }
    }



}
