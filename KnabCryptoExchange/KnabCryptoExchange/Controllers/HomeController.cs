using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using KnabCryptoExchange.Models;
using KnabCryptoExchange.Service;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;

namespace KnabCryptoExchange.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICryptoExchangeService cryptoExchangeService;

        public HomeController(ILogger<HomeController> logger, ICryptoExchangeService cryptoExchangeService)
        {
            _logger = logger;
            this.cryptoExchangeService = cryptoExchangeService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Exchange(string cryptoCode)
        {
            if (ModelState.IsValid)
            {
                ViewData["CryptoCode"] = cryptoCode;

                var results = await cryptoExchangeService.Fetch(cryptoCode);

                return View("Display", results);
            }

            return View("Index");
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