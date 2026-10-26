using Cinema_Booking_System.Data;
using Cinema_Booking_System.Models;
using Cinema_Booking_System.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string MoviesFolder =>
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "movies");

        private string SaveFile(IFormFile file)
        {
            Directory.CreateDirectory(MoviesFolder);
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            using (var stream = System.IO.File.Create(Path.Combine(MoviesFolder, fileName)))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }

        private void DeleteFile(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            var p = Path.Combine(MoviesFolder, fileName);
            if (System.IO.File.Exists(p)) System.IO.File.Delete(p);
        }

        private MovieFormViewModel BuildForm(Movie? movie = null)
        {
            var vm = new MovieFormViewModel
            {
                Movie = movie ?? new Movie { DateTime = DateTime.Now.AddDays(1) },
                Categories = _context.Categories.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList(),
                Cinemas = _context.Cinemas.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList(),
                Actors = _context.Actors.Select(a => new SelectListItem(a.Name, a.Id.ToString())).ToList()
            };
            if (movie != null)
                vm.SelectedActorIds = movie.Actors.Select(a => a.Id).ToList();
            return vm;
        }

        public IActionResult Index()
        {
            var movies = _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .ToList();
            return View(movies);
        }

        public IActionResult Create()
        {
            return View(BuildForm());
        }

        [HttpPost]
        public IActionResult Create(MovieFormViewModel vm, IFormFile? mainImg, List<IFormFile>? subImages)
        {
            var movie = vm.Movie;

            if (string.IsNullOrWhiteSpace(movie.Name) || mainImg == null)
            {
                ModelState.AddModelError("", "Name and main image are required");
                var again = BuildForm(movie);
                again.SelectedActorIds = vm.SelectedActorIds;
                return View(again);
            }

            movie.MainImg = SaveFile(mainImg);

            if (subImages != null)
            {
                foreach (var f in subImages.Where(f => f.Length > 0))
                    movie.SubImages.Add(new MovieSubImage { Img = SaveFile(f) });
            }

            movie.Actors = _context.Actors
                .Where(a => vm.SelectedActorIds.Contains(a.Id))
                .ToList();

            _context.Movies.Add(movie);
            _context.SaveChanges();
            TempData["Success"] = "Movie added successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Actors)
                .Include(m => m.SubImages)
                .FirstOrDefault(m => m.Id == id);
            if (movie == null) return NotFound();
            return View(BuildForm(movie));
        }

        [HttpPost]
        public IActionResult Edit(MovieFormViewModel vm, IFormFile? mainImg, List<IFormFile>? subImages)
        {
            var form = vm.Movie;
            var movie = _context.Movies
                .Include(m => m.Actors)
                .Include(m => m.SubImages)
                .FirstOrDefault(m => m.Id == form.Id);
            if (movie == null) return NotFound();

            movie.Name = form.Name;
            movie.Des = form.Des;
            movie.Price = form.Price;
            movie.Status = form.Status;
            movie.DateTime = form.DateTime;
            movie.CategoryId = form.CategoryId;
            movie.CinemaId = form.CinemaId;

            if (mainImg != null && mainImg.Length > 0)
            {
                DeleteFile(movie.MainImg);
                movie.MainImg = SaveFile(mainImg);
            }

            if (subImages != null)
            {
                foreach (var f in subImages.Where(f => f.Length > 0))
                    movie.SubImages.Add(new MovieSubImage { Img = SaveFile(f) });
            }

            movie.Actors.Clear();
            var actors = _context.Actors.Where(a => vm.SelectedActorIds.Contains(a.Id)).ToList();
            foreach (var a in actors) movie.Actors.Add(a);

            _context.SaveChanges();
            TempData["Success"] = "Movie updated successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.Include(m => m.SubImages).FirstOrDefault(m => m.Id == id);
            if (movie == null) return NotFound();

            DeleteFile(movie.MainImg);
            foreach (var s in movie.SubImages) DeleteFile(s.Img);

            _context.Movies.Remove(movie);
            _context.SaveChanges();
            TempData["Success"] = "Movie deleted successfully";
            return RedirectToAction("Index");
        }
    }
}