using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using lab1.Models;

namespace lab1.Controllers
{
    public class HomeController : Controller
    {
        private readonly FoodDelivery_v2Context _context;

        public HomeController(FoodDelivery_v2Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Type()
        {
            return RedirectToAction("Index", "Types");
        }

        public IActionResult Restaurant()
        {
            return RedirectToAction("Index", "Restaurants");
        }
        public IActionResult Ingredient()
        {
            return RedirectToAction("Index", "Ingredients");
        }
        public IActionResult Clients()
        {
            return RedirectToAction("Index", "Clients");
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
