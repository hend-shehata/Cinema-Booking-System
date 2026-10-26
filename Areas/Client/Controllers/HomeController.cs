using Cinema_Booking_System.Data;
using Cinema_Booking_System.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System.Areas.Client.Controllers
{
    [Area("Client")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 6;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? search, int? categoryId, int? cinemaId,
            decimal? minPrice, decimal? maxPrice, int page = 1)
        {
            var query = _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(m => m.Name.Contains(search));

            if (categoryId.HasValue)
                query = query.Where(m => m.CategoryId == categoryId);

            if (cinemaId.HasValue)
                query = query.Where(m => m.CinemaId == cinemaId);

            if (minPrice.HasValue)
                query = query.Where(m => m.Price >= minPrice);

            if (maxPrice.HasValue)
                query = query.Where(m => m.Price <= maxPrice);

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            if (page < 1) page = 1;

            var movies = query
                .OrderBy(m => m.DateTime)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var model = new MovieListViewModel
            {
                Movies = movies,
                Categories = _context.Categories.ToList(),
                Cinemas = _context.Cinemas.ToList(),
                Search = search,
                CategoryId = categoryId,
                CinemaId = cinemaId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(model);
        }
    }
}