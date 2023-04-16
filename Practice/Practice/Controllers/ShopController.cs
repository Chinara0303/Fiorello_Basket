using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        [HttpPost]
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id is null) return BadRequest();

            Product dbProduct = await GetProductById((int)id);

            if (dbProduct == null) return NotFound();

            List<BasketVM> baskets = GetDatasFromCookie();

            SetDatasToCookie(baskets, dbProduct.Id, dbProduct);

            return Ok();
        }
        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        private List<BasketVM> GetDatasFromCookie()
        {
            List<BasketVM> baskets;

            if (Request.Cookies["basket"] != null)
            {
                baskets = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                baskets = new List<BasketVM>();
            }
            return baskets;
        }
        private void SetDatasToCookie(List<BasketVM> baskets, int id, Product dbProduct)
        {

            BasketVM existProduct = baskets.FirstOrDefault(p => p.Id == id);

            if (existProduct == null)
            {
                baskets.Add(new BasketVM
                {
                    Id = dbProduct.Id,
                    Count = 1
                });
            }
            else
            {
                existProduct.Count++;
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(baskets));

        }
        public async Task<IActionResult> Search(string searchText)
        {
            if (String.IsNullOrWhiteSpace(searchText))
            {
                return Ok();
            }
            var products = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .OrderByDescending(p=>p.Id)
                .Where(p => p.Name.ToLower().Contains(searchText.ToLower()) 
                               || p.Category.Name.ToLower().Contains(searchText.ToLower()))
                .ToListAsync();
            return PartialView("_SearchPartial",products);
        }
    }
}
