namespace RealworldApp.Api.Model.Listings
{
    public class BrandingModel
    {
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Guid SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
    }
}
