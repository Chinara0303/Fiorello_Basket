using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Practice.Data;
using Practice.Models;
using Practice.ViewModels;

namespace Practice.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            IEnumerable<Slider> sliders = await _context.Sliders
                .ToListAsync();
            SliderInfo sliderInfo = await _context.SliderInfos
                .FirstOrDefaultAsync();
            IEnumerable<Product> products = await _context.Products
                .Include(p=>p.Images)
                .Take(8)
                .ToListAsync();
            IEnumerable<Category> categories = await _context.Categories
                .ToListAsync();
            About about = await _context.Abouts
                .FirstOrDefaultAsync();
            ExpertHeader expertHeader = await _context.ExpertHeaders
                .FirstOrDefaultAsync();
            IEnumerable<ExpertExpertPosition> expertExpertPosition = await _context.ExpertExpertPositions
                .Include(e=>e.Expert)
                .Include(e=>e.ExpertPosition)
                .ToListAsync();
            Subscribe subscribe = await _context.Subscribes
                .FirstOrDefaultAsync();
            IEnumerable<Blog> blogs = await _context.Blogs
                .ToListAsync();
            BlogHeader blogHeader = await _context.BlogHeaders
                .FirstOrDefaultAsync();
            IEnumerable<Author> authors = await _context.Authors
                .Include(a=>a.Says)
                .ToListAsync();
            IEnumerable<Instagram> instagrams = await _context.Instagrams
                .ToListAsync();


            HomeVM model = new()
            {
                Slider = sliders,
                SliderInfo = sliderInfo,
                Products = products,
                Categories = categories,
                About = about,
                ExpertHeader = expertHeader,
                ExpertExpertPositions = expertExpertPosition,
                Subscribe = subscribe,
                Blogs = blogs,
                BlogHeader = blogHeader,
                Authors = authors,
                Instagrams = instagrams
            };

            return View(model);
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
            List<BasketVM> baskets ;

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
        private void SetDatasToCookie(List<BasketVM> baskets,int id, Product dbProduct)
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

    }
}