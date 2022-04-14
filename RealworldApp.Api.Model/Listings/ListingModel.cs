using System.ComponentModel.DataAnnotations;

namespace RealworldApp.Api.Model.Listings
{
    public class ListingModel
    {
        [Required(ErrorMessage = "Required")]
        [MaxLength(50, ErrorMessage = "Max lenth 50 chars")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(250, ErrorMessage = "Max lenth 250 chars")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(10, ErrorMessage = "Max lenth 10 chars")]
        public string Size { get; set; }

        [Required(ErrorMessage = "Required")]
        public int Quantity { get; set; }

        public ListingModel? ListingVM { get; set; }
        public PackageModel? PackageVM { get; set; }

    }
}
