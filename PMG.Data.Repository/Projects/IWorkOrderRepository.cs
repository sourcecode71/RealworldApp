using Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Projects
{
    public interface IWorkOrderRepository
    {
        Task<bool> SaveWorkOrder(WorkOrderDTO orderDTO);
        List<WorkOrderDTO> LoadAllWorkOrders();
        Task<bool> UpdateWorkOrder(WorkOrderDTO orderDTO);
        IQueryable<WorkOrderDTO> GetFilteredWorkOrder(string strOT);
        Task<bool> UpdateWorkOrderStatus(WorkOrderDTO orderDTO);
        Task<List<WorkOrderDTO>> LoadAllWorkOrdersByEmp(string EmpId);
        Task<List<WorkOrderDTO>> WorkOrderByProjects(string PrId);
        Task<WorkOrderDTO> LoadWorkOrdersById(string wrkId);

    }
}
