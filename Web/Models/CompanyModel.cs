using System;

namespace Web.Models
{
    public class CompanyModel
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
    }

    public class ClientModel
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
    }
}
