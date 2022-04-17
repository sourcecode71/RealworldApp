using RealworldApp.Api.Model.Listings;
using RealworldApp.Client.Components.Listings;
using System.Text;
using System.Text.Json;

namespace RealworldApp.Client.Pages.Listings
{
    public partial class Listing
    {
        ListingModel lgVM = new ListingModel();
        BrandingModel bndVM = new BrandingModel();
        PackageModel pkgVM = new PackageModel();
        PackagingComponent pkgComponent;
        BrandingComponent bndComponent;


        protected async Task InvalidFormSubmitted()
        {
            if (bndComponent.CanSubmitBranding() & pkgComponent.CanSubmitPackage())
            {

            }
        }

        protected async Task ValidFormSubmitted()
        {
            var content = JsonSerializer.Serialize(lgVM);

            //var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            //var postResult = await _client.PostAsync("products", bodyContent);
            //var postContent = await postResult.Content.ReadAsStringAsync();
            //if (!postResult.IsSuccessStatusCode)
            //{
            //    throw new ApplicationException(postContent);
            //}


        }

    }
}
