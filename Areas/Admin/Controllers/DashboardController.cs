using Cinema_Booking_System.Data;
using Microsoft.AspNetCore.Mvc;

namespace Cinema_Booking_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Movies = _context.Movies.Count();
            ViewBag.Categories = _context.Categories.Count();
            ViewBag.Cinemas = _context.Cinemas.Count();
            ViewBag.Actors = _context.Actors.Count();
            return View();
        }
    }
}