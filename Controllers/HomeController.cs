using AjaxDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AjaxDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MydbContext _context;

        public HomeController(ILogger<HomeController> logger,MydbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult CallApi()
        {
            return View();
        }

        public IActionResult Address()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Register2()
        {
            return View();
        }
        public IActionResult Json()
        {
            //var categories = _context.Categories.ToList();
            return View();
        }

        public IActionResult Travel()
        {
            //var categories = _context.Categories.ToList();
            return View();
        }

        public IActionResult Travel2()
        {
            //var categories = _context.Categories.ToList();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
