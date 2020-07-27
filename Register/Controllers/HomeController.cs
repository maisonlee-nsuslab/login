using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Register.Data;
using Register.Models;

namespace Register.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly InfoContext _context;

        public HomeController(InfoContext infoContext,ILogger<HomeController> logger)
        {
            _context = infoContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
           var x= _context.Info.FirstOrDefault(m => m.login == true);
         
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
