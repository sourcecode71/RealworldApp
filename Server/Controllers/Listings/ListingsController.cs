using Microsoft.AspNetCore.Mvc;
using RealworldApp.Api.Model.Listings;

namespace RealworldApp.Server.Controllers.Listings
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ListingModel lgModel)
        {

            return Ok(lgModel);
        }
    }
}
