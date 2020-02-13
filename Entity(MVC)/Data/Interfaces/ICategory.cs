using Entity_MVC_.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entity_MVC_.Data.Interfaces
{
    public interface ICategory
    {
        IEnumerable<Category> Categories { get; }
    }
}
