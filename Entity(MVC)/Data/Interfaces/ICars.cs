using Entity_MVC_.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entity_MVC_.Data.Interfaces
{
    public interface ICars
    {
        IEnumerable<Car> GetCars { get; }
        IEnumerable<Car> GetAvCars { get; }
        Car GetCar(int Id);
    }
}
