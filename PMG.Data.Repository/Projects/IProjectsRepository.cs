using Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Projects
{
    public interface IProjectsRepository
    {
        string  GetProjectNumber(ProjectDto projectDto);
        string GetPmBudgetNumber(ProjectApprovalDto projectDto);
        Task<List<ProjectDto>> GetProjectBySearch(string SearchTag); 
        Task<bool> SubmitBudget(ProjectApprovalDto approvalDto);
        Task<bool> ApprovalBudget(ProjectApprovalDto approvalDto);
        Task<List<ProjectApprovalDto>> LoadProjectBudgetAcitivies(string projectName);
        Task<List<ClientDTO>> GetAllClient();
        Task<List<ProjectDto>> GetAllProjects(string empId);
        Task<List<ProjectDto>> GetAllActiveProjects();
        Task<bool> UpdateProjectStatus(ProjectCorrentStatusDTO statusDTO);
    }
}
