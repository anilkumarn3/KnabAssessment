using System.Diagnostics;
using KnabCryptoViewer.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;

namespace KnabCryptoViewer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CryptocurrencyReader reader;

        public HomeController(ILogger<HomeController> logger, CryptocurrencyReader reader)
        {
            _logger = logger;
            this.reader = reader;
        }

        public IActionResult Index()
        {
            List<string> currencies = new List<string> { "USD", "EUR", "BRL", "GBP", "AUD" };
            List<string> currencies1 = new List<string> { "USD" };
            reader.FetchBitcoinValue("BTC", currencies1).Wait();
            return View();
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
