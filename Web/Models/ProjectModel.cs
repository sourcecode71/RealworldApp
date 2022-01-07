using System;
using System.Globalization;
using System.Linq;

namespace Web.Models
{
    public class ProjectModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int SelfProjectId { get; set; }
        public string Client { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Schedule { get; set; }
        public double Progress { get; set; }
        public double Budget { get; set; }
        public double Paid { get; set; }
        public double Balance { get; set; }
        public double Factor { get; set; }
        public string EStatus { get; set; }
        public string Status { get; set; }
        public string Engineering { get; set; }
        public string Drawing { get; set; }
        public string Approval { get; set; }
        public string EmployeesNames { get; set; }
        public string AdminDelayedComment { get; set; }
        public string AdminModifiedComment { get; set; }
        public string EmployeeDelayedComment { get; set; }
        public string EmployeeModifiedComment { get; set; }
        public string BudgetFormat { get { return CreateFormatForCurrency(Budget); } set { } }
        public string PaidFormat { get { return CreateFormatForCurrency(Paid); } set { } }
        public string BalanceFormat { get { return CreateFormatForCurrency(Balance); } set { } }
        public string FactorFormat { get { return CreateFormatForCurrency(Factor); } set { } }

        public string CreateFormatForCurrency(double amount)
		{
            return String.Format(CultureInfo.InvariantCulture,
                                 "{0:0,0.00}", amount);
        }
    }
}