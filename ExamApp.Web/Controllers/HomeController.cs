using ExamApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IExamService _examService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IExamService examService, ILogger<HomeController> logger)
        {
            _examService = examService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Session'dan kullanýcý bilgilerini al
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.IsLoggedIn = !string.IsNullOrEmpty(ViewBag.Username);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Core.Models.ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}