using Domain;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Application.DTOs
{
    public class ProjectDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SelfProjectId { get; set; }
        public int Year { get; set; }
        public double Week { get; set; }
        public string ProjectNo { get; set; }
        public string Client { get; set; }
        public string ClientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Schedule { get; set; }
        public double Progress { get; set; }
        public double Budget { get; set; }
        public double Paid { get; set; }
        public double Balance { get; set; }
        public double Factor { get; set; }
        public string EStatus { get; set; }
        public string Status { get; set; }
        public string AdminDelayedComment { get; set; }
        public string AdminModifiedComment { get; set; }
        public string EmployeeDelayedComment { get; set; }
        public string EmployeeModifiedComment { get; set; }
        public string Engineering { get; set; }
        public string Drawing { get; set; }
        public double BudgetHours { get; set; }
        public double DrawingHours { get; set; }
        public double EngineeringHours { get; set; }
        public string Approval { get; set; }
        public string BudgetApprovalStr { get; set; }
        public string EmployeesNames { get; set; }
        public string Remarks { get; set; }
        public ProjectStatusDTO ProejctStatus { get; set; }
        public ICollection<ProjectEmployee> ProjectEmployees { get; set; }
        public List<ProjectActivityDto> Activities { get; set; } = new List<ProjectActivityDto>();
        public List<EmployeeDto> Employees { get; set; }
        public List<ProjectEmp> Engineers { get; set; }
        public List<ProjectEmp> Drawings { get; set; }
    }

    public class ProjectApprovalDto {
        public string Id { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ApporvalSatus { get; set; }
        public string ProjectNo { get; set; }
        public string BudegtNo { get; set; }
        public string ClientName { get; set; }
        public DateTime ApprovalDate { get; set; }
        public String ApprovalDateStr { get; set; }
        public String ApprovalSetUser { get; set; }
        public int ApprovalStatus { get; set; }
        public string ApprovalStatusStr { get; set; }
        public int Status { get; set; }
        public double Budget { get; set; }
        public DateTime BudgetSubmitDate { get; set; }
        public string BudgetSubmitDateStr { get; set; }
        public double? ApprovedBudget { get; set; }
        public double Balance { get; set; }
        public string Comments { get; set; }
    }

    public class ProjectEmp
    {
        public string Id { get; set; }
        public double hour { get; set; }
    }

}