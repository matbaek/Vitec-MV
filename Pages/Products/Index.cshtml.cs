using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Vitec_MV.Models;

namespace Vitec_MV.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly Vitec_MV.Models.ProductContext _context;

        public IndexModel(Vitec_MV.Models.ProductContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; }

        public async Task OnGetAsync()
        {
            Product = await _context.Product.ToListAsync();
        }
    }
}
