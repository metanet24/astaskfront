using FrontEndProject_Connect_BackEnd.Data;
using FrontEndProject_Connect_BackEnd.Models;
using FrontEndProject_Connect_BackEnd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrontEndProject_Connect_BackEnd.Controllers
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
            List<Category> categories = await _context.Categories.ToListAsync();
            List<Product> products = await _context.Products
                                                   .Include(m=>m.ProductImages)
                                                   .ToListAsync();

            HomeVM model = new()
            {
                Categories = categories,
                Products = products
            };
            return View(model);
        }
    }
}
