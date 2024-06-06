using FrontEndProject_Connect_BackEnd.Data;
using Microsoft.AspNetCore.Mvc;

namespace FrontEndProject_Connect_BackEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
