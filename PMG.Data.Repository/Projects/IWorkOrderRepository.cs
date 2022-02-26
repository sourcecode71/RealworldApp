using Application.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Projects
{
    public interface IWorkOrderRepository
    {
        Task<bool> SaveWorkOrder(WorkOrderDTO orderDTO);
        IQueryable<WorkOrderDTO> LoadAllWorkOrders();
        Task<bool> UpdateWorkOrder(WorkOrderDTO orderDTO);
        IQueryable<WorkOrderDTO> GetFilteredWorkOrder(string strOT);

        Task<bool> SaveInvoice(InvoiceDTO invDTO);
        IQueryable<InvoiceDTO> GetAllInvoices();

    }
}
