using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Practice.Data;
using Practice.Models;
using Practice.ViewModels;

namespace Practice.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        public BasketController(AppDbContext context)
        {
            _context = context;
        }
        public async  Task<IActionResult> Index()
        {
           List<BasketVM> baskets = GetDatasFromCookie();
            List<BasketDetailVM> basketDetailVMs = new(); 
            foreach (var item in baskets)
            {
               Product dbProduct = _context.Products
                    .Include(p=>p.Images)
                    .FirstOrDefault(p => p.Id == item.Id);
                basketDetailVMs.Add(new BasketDetailVM()
                {
                    Id = dbProduct.Id,
                    Name =  dbProduct.Name,
                    Price = dbProduct.Price,
                    Image = dbProduct.Images.FirstOrDefault(i=>i.IsMain).Image,
                    Count = item.Count,
                    Total = dbProduct.Price * item.Count
                });
            }
            return View(basketDetailVMs);
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

        public IActionResult DeleteDataFromBasket(int? id)
        {
            if (id is null) return BadRequest();

            var baskets = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            var deletedProduct  = baskets.FirstOrDefault(b => b.Id == id);
            baskets.Remove(deletedProduct);

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(baskets));

            return Ok(baskets);
        }

        public IActionResult IncrementProductCount(int? id)
        {
            if (id is null) return BadRequest();
            var baskets = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            var count =  baskets.FirstOrDefault(b => b.Id == id).Count++;
            
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(baskets));

            return Ok(count);
        }
        public IActionResult DecrementProductCount(int? id)
        {
            if (id is null) return BadRequest();
            var baskets = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            var count = baskets.FirstOrDefault(b => b.Id == id).Count--;

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(baskets));

            return Ok(count);
        }
    }
}
