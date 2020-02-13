using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Entity_MVC_.Models;
using Entity_MVC_.Data.Interfaces;
using Entity_MVC_.ViewModels;

namespace Entity_MVC_.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICars _cars;
        private readonly ICategory _category;

        public HomeController(ICars cars, ICategory category)
        {
            _cars = cars;
            _category = category;
        }

        public ViewResult Index()
        {
            var cars = new HomeViewModel
            {
                CarAvailable = _cars.GetAvCars
            };
            return View(cars);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
