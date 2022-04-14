using System.ComponentModel.DataAnnotations;

namespace RealworldApp.Api.Model.Listings
{
    public class PackageModel
    {
        [Required(ErrorMessage = "Required")]
        [MaxLength(50, ErrorMessage = "Max lenth 50 chars")]
        public string Lenth { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(50, ErrorMessage = "Max lenth 50 chars")]
        public string Weight { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(50, ErrorMessage = "Max lenth 50 chars")]
        public string Height { get; set; }
    }
}
