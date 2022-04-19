using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealworldApp.Data.Model.Listings
{
    public class SubCategory
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar"), Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public bool IsDeleted { get; set; }
    }
}
