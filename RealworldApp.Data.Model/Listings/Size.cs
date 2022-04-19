using System.ComponentModel.DataAnnotations;

namespace RealworldApp.Data.Model.Listings
{
    public class Size
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}
