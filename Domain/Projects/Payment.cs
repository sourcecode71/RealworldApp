using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Projects
{
    public class Payment : BasedModel
    {
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
        [MaxLength(15)]
        public string InvoiceNo { get; set; }
        [MaxLength(15)]
        public Guid WorkOrderId { get; set; }
        public double PayAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        [MaxLength(250)]
        public string Remarks { get; set; }
    }
}
