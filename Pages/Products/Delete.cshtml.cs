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
    public class DeleteModel : PageModel
    {
        private readonly Vitec_MV.Models.ProductContext _context;

        public DeleteModel(Vitec_MV.Models.ProductContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }
        public string ConcurrencyErrorMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(int id, bool? concurrencyError)
        {
            Product = await _context.Product.FirstOrDefaultAsync(m => m.ID == id);

            if (Product == null)
            {
                return NotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ConcurrencyErrorMessage = "The product you attempted to delete "
                  + "was modified by another user after you selected delete. "
                  + "The delete operation was canceled and the current values in the "
                  + "database have been displayed. If you still want to delete this "
                  + "record, click the Delete button again.";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                if (await _context.Product.AnyAsync(
                    m => m.ID == id))
                {
                    _context.Product.Remove(Product);
                    await _context.SaveChangesAsync();
                }
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToPage("./Delete",
                    new { concurrencyError = true, id = id });
            }
        }
    }
}
