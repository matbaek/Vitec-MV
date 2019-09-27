using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vitec_MV.Models;

namespace Vitec_MV.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly Vitec_MV.Models.ProductContext _context;

        public EditModel(Vitec_MV.Models.ProductContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Product.FirstOrDefaultAsync(m => m.ID == id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var productToUpdate = await _context.Product
                .Include(p => p.ID)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (productToUpdate == null)
            {
                return HandleDeletedProduct();
            }

            _context.Entry(productToUpdate).Property("RowVersion").OriginalValue = Product.RowVersion;

            if (await TryUpdateModelAsync<Product>(
               productToUpdate,
               "Produkter",
               s => s.Titel, s => s.Description, s => s.Price, s => s.ImageURL))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Product)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save. " +
                            "The product was deleted by another user.");
                        return Page();
                    }

                    var dbValues = (Product)databaseEntry.ToObject();
                    SetDbErrorMessage(dbValues, clientValues, _context);

                    Product.RowVersion = (byte[])dbValues.RowVersion;
                    ModelState.Remove("Product.RowVersion");
                }
            }
            return Page();
        }

        private IActionResult HandleDeletedProduct()
        {
            var deletedProduct = new Product();
            ModelState.AddModelError(string.Empty,
                "Unable to save. The product was deleted by another user.");
            return Page();
        }

        private void SetDbErrorMessage(Product dbValues,
                Product clientValues, ProductContext context)
        {

            if (dbValues.Titel != clientValues.Titel)
            {
                ModelState.AddModelError("Product.Titel",
                    $"Current value: {dbValues.Titel}");
            }
            if (dbValues.Description != clientValues.Description)
            {
                ModelState.AddModelError("Product.Description",
                    $"Current value: {dbValues.Description:c}");
            }
            if (dbValues.Price != clientValues.Price)
            {
                ModelState.AddModelError("Product.Price",
                    $"Current value: {dbValues.Price:d}");
            }
            if (dbValues.ImageURL != clientValues.ImageURL)
            {
                ModelState.AddModelError("Product.ImageURL",
                    $"Current value: {dbValues.ImageURL:e}");
            }

            ModelState.AddModelError(string.Empty,
                "The record you attempted to edit "
              + "was modified by another user after you. The "
              + "edit operation was canceled and the current values in the database "
              + "have been displayed. If you still want to edit this record, click "
              + "the Save button again.");
        }

        //private bool ProductExists(int id)
        //{
        //    return _context.Product.Any(e => e.ID == id);
        //}
    }
}
