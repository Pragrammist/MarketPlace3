using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HukSleva.Models;
using UserDataBase;
namespace HukSleva.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        UserDb _userDb;
        public HomeController(ILogger<HomeController> logger, UserDb userDb)
        {
            _logger = logger;
            _userDb = userDb;
        }

        public IActionResult Index()
        {
            
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Catalog", "Books");
            }

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
