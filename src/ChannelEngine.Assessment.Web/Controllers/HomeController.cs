using ChannelEngine.Assessment.Application.DTOs;
using ChannelEngine.Assessment.Application.Services;
using ChannelEngine.Assessment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelEngine.Assessment.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMarketingApplicationService _marketingApplicationService;

        public HomeController(IMarketingApplicationService marketingApplicationService)
        {
            _marketingApplicationService = marketingApplicationService;
        }

        public async Task<List<BestSellerProductDto>> Index(CancellationToken cancellationToken)
        {
            var bestSellerProducts = await _marketingApplicationService.GetBestSellerProductsAsync(5, cancellationToken);

            return bestSellerProducts;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
