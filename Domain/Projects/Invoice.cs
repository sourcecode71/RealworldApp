using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Projects
{
    public class Invoice : BasedModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid WorkOrderId { get; set; }
        public virtual WorkOrder WorkOrder  { get;set;}

        [MaxLength(100)]
        public string WorkOrderNo { get; set; }
        public string ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public double PartialBill { get; set; }
        public double InvoiceBill { get; set; }
        public double Balance { get; set; }
        [MaxLength(100)]
        public string InvoiceNumber { get; set; }
        [MaxLength(250)]
        public string Remarks { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}
