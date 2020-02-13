using Entity_MVC_.Data.EFContext;
using Entity_MVC_.Data.Interfaces;
using Entity_MVC_.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entity_MVC_.Data.Repository
{
    public class CategoryRepository: ICategory
    {
        private readonly EFDbContext _context;

        public CategoryRepository(EFDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> Categories => _context.Categories;
    }
}
