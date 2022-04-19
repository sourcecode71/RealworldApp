using System.ComponentModel.DataAnnotations;

namespace RealworldApp.Data.Model.Listings
{
    public class Category
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string? CatgoryName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
