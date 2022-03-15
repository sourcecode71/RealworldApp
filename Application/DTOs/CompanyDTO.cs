using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class ClientDTO
    {
        public Guid Id { get; set; }
        [MaxLength(200), Required]
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
    public class CompanyDTO
    {
        public Guid Id { get; set; }
        [MaxLength(200), Required]
        public string Name { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        [MaxLength(200)]
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }

        public string SetUser { get; set; }
    }
}
