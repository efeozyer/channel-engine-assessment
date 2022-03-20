using ChannelEngine.Assessment.Application.Services;
using ChannelEngine.Assessment.Web.Models;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var viewModel = new BestSellerProductsViewModel();

            viewModel.Products = await _marketingApplicationService.GetBestSellerProductsAsync(5, cancellationToken);

            return View(viewModel);
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
