using ChannelEngine.Assessment.Application.Services;
using ChannelEngine.Assessment.Web.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelEngine.Assessment.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IWarehouseApplicationService _warehouseApplicationService;

        public ProductController(IWarehouseApplicationService warehouseApplicationService)
        {
            _warehouseApplicationService = warehouseApplicationService;
        }

        [HttpPatch]
        [Route("{productId}")]
        public async Task<IActionResult> PatchAsync([FromRoute] string productId, [FromBody] UpdateProductQuantityRequest payload, CancellationToken cancellationToken)
        {
            var response = await _warehouseApplicationService.UpdateProductQuantityAsync(productId, payload.Quantity, cancellationToken);

            if (!response)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }
    }
}
