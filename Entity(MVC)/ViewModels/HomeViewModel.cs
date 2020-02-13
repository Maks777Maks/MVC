using Entity_MVC_.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entity_MVC_.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Car> CarAvailable { get; set; }
    }
}
