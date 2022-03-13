using Application.DTOs;
using Domain.Projects;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            var invDTO = await (from inv in _context.Invoice
                         join wrk in _context.WorkOrder on inv.WorkOrderId equals wrk.Id
                         join prj in _context.Projects on wrk.ProjectId equals prj.Id
                         orderby inv.Id descending
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
                             Remarks = inv.Remarks
                         }).ToListAsync();

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

                    return State == 1;

                }
                catch (Exception ex)
                {
                    await transaction.CommitAsync();
                    throw ex;
                } 
            }
        }
    }
}
