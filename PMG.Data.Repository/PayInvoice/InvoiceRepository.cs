using Application.DTOs;
using Domain.Projects;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data;

namespace PMG.Data.Repository.PayInvoice
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DataContext _context;
        public InvoiceRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<List<InvoiceDTO>> GetAllInvoice()
        {
            try
            {
                DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "dbo.ActiveInvoice";
                cmd.CommandType = CommandType.StoredProcedure;

                List<InvoiceDTO> invDTO = new List<InvoiceDTO>();

                _context.Database.OpenConnection();
                using (DbDataReader rd = await cmd.ExecuteReaderAsync())
                {
                    while (rd.Read())
                    {

                        InvoiceDTO inv = new InvoiceDTO()
                        {
                            Id = rd.GetValue("id").ToString(),
                            WorkOrderId = rd.GetValue("wrkId").ToString(),
                            WorkNo = rd.GetValue("WorkOrderNo").ToString(),
                            WorkOrderName = rd.GetValue("ConsWork").ToString(),
                            OTName = rd.GetValue("OTDescription").ToString(),
                            ProjectName = rd.GetValue("Name").ToString(),
                            ClientName = rd.GetValue("ClientName").ToString(),
                            CompanyName = rd.GetValue("CompanyName").ToString(),
                            OriginalBudget = Convert.ToDouble(rd.GetValue("OriginalBudget").ToString()),
                            ApprovedBudget = Convert.ToDouble(rd.GetValue("ApprovedBudget").ToString()),
                            Balance = Convert.ToDouble(rd.GetValue("Balance").ToString()),
                            SpentHour = Convert.ToDouble(rd.GetValue("SpentHour").ToString()),
                            BudgetHour = Convert.ToDouble(rd.GetValue("BudgetHour").ToString()),
                            InvoiceBill = Convert.ToDouble(rd.GetValue("BudgetHour").ToString()),
                            PartialBill = Convert.ToDouble(rd.GetValue("BudgetHour").ToString()),
                            InvoiceNumber = rd.GetValue("BudgetHour").ToString(),
                            ApprovedDateStr = rd.GetValue("ApprovalDate").ToString(),
                            InvoiceDateStr = rd.GetValue("InvoiceDate").ToString(),
                            DueDateStr = rd.GetValue("EndDate").ToString()

                        };

                        invDTO.Add(inv);
                    }
                }

                return invDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public async Task<List<InvoiceDTO>> GetPendingInvoice()
        {
            var invs = _context.Payment.Select(p3=>p3.InvoiceNo).ToList();

            var invDTO = await (from inv in _context.Invoice
                                join wrk in _context.WorkOrder on inv.WorkOrderId equals wrk.Id
                                join prj in _context.Projects on wrk.ProjectId equals prj.Id
                                join c in _context.Company on prj.CompanyId equals c.Id into cc
                                from cm in cc.DefaultIfEmpty()
                                join l in _context.Clients on cm.ClientId equals l.Id into ll
                                from cl in ll.DefaultIfEmpty()
                                orderby inv.SetDate descending
                                select new InvoiceDTO
                                {
                                    Id = inv.Id.ToString(),
                                    WorkOrderId = inv.WorkOrderId.ToString(),
                                    WorkNo = wrk.WorkOrderNo,
                                    WorkOrderName = wrk.ConsWork,
                                    OTName = wrk.OTDescription,
                                    ProjectName = prj.Name,
                                    PartialBill = inv.PartialBill,
                                    InvoiceBill = inv.InvoiceBill,
                                    InvoiceNumber = inv.InvoiceNumber,
                                    InvoiceDate = inv.InvoiceDate,
                                    InvoiceDateStr = inv.InvoiceDate.ToString("MM/dd/yyyy"),
                                    Balance = inv.Balance,
                                    Remarks = inv.Remarks,
                                    OriginalBudget = wrk.OriginalBudget,
                                    ApprovedBudget = wrk.ApprovedBudget,
                                    ApprovedDateStr = wrk.ApprovalDate.ToString("MM/dd/yyyy"),
                                    CompanyName = cm.Name,
                                    ClientName = cl.Name,
                                    DueDateStr = wrk.EndDate.ToString("MM/dd/yyyy")

                                }).Where(p=> !invs.Contains(p.InvoiceNumber)).ToListAsync();

            return invDTO;
        }

        public Task<List<InvoiceDTO>> GetProjectByInvoice()
        {
            throw new NotImplementedException();
        }

        public Task<List<InvoiceDTO>> GetWorkOrderByInvoice()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveInvoice(InvoiceDTO invDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var wrkOD = await _context.WorkOrder.FirstOrDefaultAsync(w => w.Id == new Guid(invDTO.WorkOrderId));

                    double PrevBalance = 0;

                    if(wrkOD.BudgetStatus == 0)
                    {
                        return false;
                    }

                    if(wrkOD != null)
                    {
                       PrevBalance = wrkOD.Balance;
                       wrkOD.Balance = wrkOD.Balance - invDTO.InvoiceBill;
                    }


                    var invData = new Invoice
                    {
                        Id = Guid.NewGuid(),
                        WorkOrderId = new Guid(invDTO.WorkOrderId),
                        WorkOrderNo = invDTO.WorkNo,
                        ProjectId = invDTO.ProjectId,
                        PartialBill = invDTO.PartialBill,
                        InvoiceBill = invDTO.InvoiceBill,
                        Balance = PrevBalance- invDTO.InvoiceBill,
                        InvoiceDate = invDTO.InvoiceDate,
                        InvoiceNumber = invDTO.InvoiceNumber,
                        Remarks = invDTO.Remarks,
                        SetUser = invDTO.SetUser,
                        SetDate = DateTime.Now
                    };

                    _context.Invoice.Add(invData);

                    var State = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;

                }
                catch (Exception ex)
                {
                    await transaction.CommitAsync();
                    throw ex;
                } 
            }
        }

        public async Task<bool> SavePayBill(PaymentDto dTO)
        {
            try
            {
                Payment payment = new Payment
                {
                    Id = new Guid(),
                    InvoiceId = new Guid(dTO.InvoiceId),
                    InvoiceNo = dTO.InvoiceNo,
                    WorkOrderId = new Guid(dTO.WorkOrderId),
                    PayAmount = dTO.PayAmount,
                    PaymentDate = dTO.PayDate,
                    Remarks = dTO.Remarks,
                    SetDate = DateTime.Now,
                    SetUser = dTO.SetUser,
                };

                _context.Payment.Add(payment);

                var Status = await _context.SaveChangesAsync();

                return Status > 0;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<PaymentDto>> GetAllPayment()
        {
            try
            {
                var pay= await (from p in _context.Payment 
                         join i in _context.Invoice on p.InvoiceId equals i.Id
                         join w in _context.WorkOrder on p.WorkOrderId equals w.Id
                         join pj in _context.Projects on w.ProjectId equals pj.Id
                         orderby p.SetDate descending
                         select new PaymentDto
                         {
                             Id =p.Id.ToString(),
                             WorkNo = w.WorkOrderNo,
                             ProjectNo = pj.ProjectNo,
                             WorkOrderName=w.ConsWork,
                             ApprovedBudget=w.ApprovedBudget,
                             InvoiceNo=i.InvoiceNumber,
                             InvoiceBill = i.InvoiceBill,
                             InvoiceDateStr=i.InvoiceDate.ToString("MM/dd/yyyy"),
                             PayAmount=p.PayAmount,
                             PayDateStr=p.PaymentDate.ToString("MM/dd/yyyy")

                         }).ToListAsync();

                return pay;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
