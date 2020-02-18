using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Entity_MVC_.Data.Interfaces;
using Entity_MVC_.ViewModels;
using Entity_MVC_.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace Entity_MVC_.Controllers
{
    
    public class CarsController : Controller
    {
        private readonly ICars _cars;
        private readonly ICategory _category;

        public CarsController(ICars cars, ICategory category)
        {
            _cars = cars;
            _category = category;
        }

        [Authorize(Roles = "User")]
        [Route("Cars/ListCars")]
        [Route("Cars/ListCars/{category}")]
        public ViewResult ListCars(string category, string id)
        {

            IEnumerable<Car> cars = null;
            string carCategory = "";
            if (string.IsNullOrEmpty(category))
            {
                cars = _cars.GetCars.OrderBy(i => i.Id);
            }
            else
            {
                cars = _cars.GetCars
                    .Where(x => x.Category.CategoryName.ToLower() == category.ToLower());              
                carCategory = category;
            }

            var carObj = new CarsListViewModel
            {
                GetCars = cars,
                CarCategory = carCategory
            };

            ViewBag.Title = "All Cars";
            return View(carObj);

            //ViewBag.Title = "All Cars";
            //CarsListViewModel obj = new CarsListViewModel();
            //obj.GetCars = _cars.GetCars;
            //obj.CarCategory = "auto";

            //return View(obj);

            //var cars = _cars.GetCars;

            //return View(cars);
        }
    }
}