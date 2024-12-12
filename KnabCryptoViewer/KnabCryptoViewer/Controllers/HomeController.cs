using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using KnabCryptoViewer.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;

namespace KnabCryptoViewer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CryptoExchangeService cryptoExchangeService;

        public HomeController(ILogger<HomeController> logger, CryptoExchangeService cryptoExchangeService)
        {
            _logger = logger;
            this.cryptoExchangeService = cryptoExchangeService;
        }

        public async Task<IActionResult> Index(string id)
        {
            var results = await cryptoExchangeService.Fetch(id);

            return Ok(results);
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