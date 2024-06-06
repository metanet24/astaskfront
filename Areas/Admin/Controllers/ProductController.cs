using FrontEndProject_Connect_BackEnd.Data;
using FrontEndProject_Connect_BackEnd.Helpers.Extentions;
using FrontEndProject_Connect_BackEnd.Models;
using FrontEndProject_Connect_BackEnd.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrontEndProject_Connect_BackEnd.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
		{
			List<Product> products = await _context.Products.Include(m => m.ProductImages)
                                                            .Include(m => m.Category)
                                                            .ToListAsync();

			List<ProductVM> model = products.Select(m=> new ProductVM { Id = m.Id,Title = m.Title,Description = m.Description,Price = m.Price,Category = m.Category.Name}).ToList();
			return View(model);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id is null) return BadRequest();

			Product product = await _context.Products.Include(m=>m.ProductImages)
												     .Include(m=> m.Category)
													 .Where(m=>!m.SoftDelete)
													 .FirstOrDefaultAsync(m=>m.Id == id);

			if(product is null) return NotFound();

            foreach (var item in product.ProductImages)
            {
                string path = Path.Combine(_env.WebRootPath, "img", item.Name);

                path.DeleteFileFromLocal();
            }

			_context.Products.Remove(product);

			await _context.SaveChangesAsync();

			return RedirectToAction("Index");


        }


		[HttpGet]
		public async Task<IActionResult> Detail(int? id)
		{
			if(id is null) return BadRequest();

			Product product = await _context.Products.Include(m => m.ProductImages)
													 .Include(m => m.Category)
													 .Where(m => !m.SoftDelete)
													 .FirstOrDefaultAsync(m => m.Id == id);

			if(product is null) return NotFound();


			List<ProductImageVM> productImages = new();

            foreach (var item in product.ProductImages)
            {
				productImages.Add(new ProductImageVM
				{
					Image = item.Name,
					IsMain = item.IsMain
				});
            }

			ProductDetailVM model = new()
			{
				Name = product.Title,
				Description = product.Description,
				Price = product.Price,
				Category = product.Category.Name,
				Images = productImages
			};

			return View(model);


        }


		[HttpGet]
		public async Task<IActionResult> Edit(int? id)
		{
			ViewBag.Categories = await _context.Categories.ToListAsync();

			if(id is null) return BadRequest();

			Product product = await _context.Products.Include(m => m.ProductImages)
													 .Include(m => m.Category)
													 .FirstOrDefaultAsync(m => m.Id == id);

			if(product is null) return NotFound();

            List<ProductImageVM> productImage = new();

            foreach (var item in product.ProductImages)
            {
                productImage.Add(new ProductImageVM
                {
                    Image = item.Name,
                    IsMain = item.IsMain
                });
            }

            return View(new ProductEditVM { Name = product.Title, Description = product.Description, Price = product.Price, Images = productImage, Category = product.Category.Name });


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEditVM productEditVM, int? id)
        {


            if (id is null) return BadRequest();

            Product existProduct = await _context.Products.Include(m => m.ProductImages)
                                                          .Include(m => m.Category)
                                                          .Where(m => !m.SoftDelete)
                                                          .FirstOrDefaultAsync(m => m.Id == id);

            if (existProduct is null) return NotFound();


            existProduct.Title = productEditVM.Name;
            existProduct.Description = productEditVM.Description;
            existProduct.Price = productEditVM.Price;
            existProduct.CategoryId = productEditVM.CategoryId;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
    }
}
