using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.Models;
using Practice.ViewModels;

namespace Practice.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        public ShopController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int count = await _context.Products.Where(p=>!p.SoftDelete).CountAsync();
            ViewBag.Count = count;

            IEnumerable<Product> products = await _context.Products
                .Include(p=>p.Images)
                .Include(p=>p.Category)
                .Where(p=>!p.SoftDelete)
                .Take(4)
                .ToListAsync();

            IEnumerable<Category> categories = await _context.Categories
                .Where(p => !p.SoftDelete)
                .ToListAsync();

            ViewBag.Categories = categories;

            return View(products);
        }
        public async Task<IActionResult> LoadMore(int skip)
        {
            IEnumerable<Product> products = await _context.Products
              .Include(p => p.Images)
              .Where(p => !p.SoftDelete)
              .Skip(skip)
              .Take(4)
              .ToListAsync();

            return PartialView("_ProductsPartial", products);
        }
    }
}
