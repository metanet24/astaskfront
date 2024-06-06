using FrontEndProject_Connect_BackEnd.Data;
using FrontEndProject_Connect_BackEnd.Helpers.Extentions;
using FrontEndProject_Connect_BackEnd.Models;
using FrontEndProject_Connect_BackEnd.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace FrontEndProject_Connect_BackEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context,
                                 IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories.ToListAsync();    

            List<CategoryVM> model = categories.Select(m=> new CategoryVM { Id = m.Id,Name = m.Name,Image = m.Image}).ToList(); 

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            if (category is null) return NotFound();

            CategoryDetailVM model = new()
            {
                Name = category.Name,
                Image = category.Image,
            };

            return View(model);
                                                         
        }




        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            Category category = await _context.Categories.Where(m=>m.Id == id)
                                                         .Include(m=>m.Products)
                                                         .FirstOrDefaultAsync();

            if (category is null) return NotFound();

            
            category.Image.DeleteFile(_env.WebRootPath, "assets", "images");
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        

    }
}
