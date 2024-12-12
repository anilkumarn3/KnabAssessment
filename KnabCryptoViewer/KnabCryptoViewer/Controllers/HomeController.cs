using System.Diagnostics;
using System.Threading.Tasks;
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

        public async Task<IActionResult> Index(string id)
        {
            List<string> currencies = new List<string> { "USD", "EUR", "BRL", "GBP", "AUD" };

            var tasks = new List<Task<CryptoValue>>();

            foreach (var currency in currencies)
            {
                tasks.Add(reader.FetchBitcoinValue(id, currency));
            }

            var results = await Task.WhenAll(tasks);

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
