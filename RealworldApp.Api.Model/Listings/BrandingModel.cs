using System.ComponentModel.DataAnnotations;

namespace RealworldApp.Api.Model.Listings
{
    public class BrandingModel
    {
        [Required(ErrorMessage = "Required")]
        public Guid BrandId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string BrandName { get; set; }

        [Required(ErrorMessage = "Required")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Required")]
        public Guid SubCategoryId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string SubCategoryName { get; set; }
    }
}
