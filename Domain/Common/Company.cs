using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common
{
    public class Client : BasedModel
    {
        public Client()
        {
            Id= Guid.NewGuid();
            IsActive = true;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(100),Required]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Address { get; set; }
        [Required]
        [MaxLength(100)]
        public string ContactName { get; set; }
        [MaxLength(50)]
        public string email { get; set; }
        [MaxLength(50)]
        public string phone { get; set; }
        public bool IsActive { get; set; }
    }
    public class Company : BasedModel
    {
        public Company()
        {
            IsActive= true;
            Id= Guid.NewGuid();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(200), Required]
        public string Name { get; set; }
        public Guid ClientId { get; set; }
        public virtual Client Client { get; set; }
        [MaxLength(100)]
        public string ContactName { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }
        [Required]
        public bool IsActive { get; set; }

    }
}
