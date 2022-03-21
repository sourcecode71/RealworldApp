using System;

namespace Application.DTOs
{
    public class PaymentDto
    {
        public string Id { get; set; }
        public double PayAmount { get; set; }
        public DateTime PayDate { get; set; }
        public string PayDateStr { get; set; }
        public string WorkOrderId { get; set; }
        public string CompanyName { get; set; }
        public string WorkNo { get; set; }
        public string ProjectId { get; set; }
        public string ProjectNo { get; set; }
        public string InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public double InvoiceBill { get; set; }
        public double Balance { get; set; }
        public string OTName { get; set; }
        public string ProjectName { get; set; }
        public string WorkOrderName { get; set; }
        public string ClientName { get; set; }
        public double OriginalBudget { get; set; }
        public double ApprovedBudget { get; set; }
        public string ApprovedDateStr { get; set; }
        public string Remarks { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceDateStr { get; set; }
        public string SetUser { get; set; }
        public string DueDateStr { get; set; }
        public DateTime SetDate { get; set; }


    }
}
