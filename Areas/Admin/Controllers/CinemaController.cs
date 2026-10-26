using Cinema_Booking_System.Data;
using Cinema_Booking_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CinemaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cinemas = _context.Cinemas.Include(c => c.Movies).ToList();
            return View(cinemas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Cinema cinema, IFormFile? img)
        {
            if (string.IsNullOrWhiteSpace(cinema.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
                return View(cinema);
            }

            if (img != null && img.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cinemas", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                using (var stream = System.IO.File.Create(path))
                {
                    img.CopyTo(stream);
                }
                cinema.Img = fileName;
            }

            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
            TempData["Success"] = "Cinema added successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema == null) return NotFound();
            return View(cinema);
        }

        [HttpPost]
        public IActionResult Edit(Cinema cinema, IFormFile? img)
        {
            var old = _context.Cinemas.AsNoTracking().FirstOrDefault(c => c.Id == cinema.Id);
            if (old == null) return NotFound();

            if (img != null && img.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cinemas");
                Directory.CreateDirectory(folder);

                using (var stream = System.IO.File.Create(Path.Combine(folder, fileName)))
                {
                    img.CopyTo(stream);
                }

                // امسحي الصورة القديمة
                if (!string.IsNullOrEmpty(old.Img))
                {
                    var oldPath = Path.Combine(folder, old.Img);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }
                cinema.Img = fileName;
            }
            else
            {
                cinema.Img = old.Img; // سيبي الصورة القديمة
            }

            _context.Cinemas.Update(cinema);
            _context.SaveChanges();
            TempData["Success"] = "Cinema updated successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var cinema = _context.Cinemas.Include(c => c.Movies).FirstOrDefault(c => c.Id == id);
            if (cinema == null) return NotFound();

            if (cinema.Movies.Any())
            {
                TempData["Error"] = "Can't delete a cinema that has movies";
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(cinema.Img))
            {
                var p = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cinemas", cinema.Img);
                if (System.IO.File.Exists(p)) System.IO.File.Delete(p);
            }

            _context.Cinemas.Remove(cinema);
            _context.SaveChanges();
            TempData["Success"] = "Cinema deleted successfully";
            return RedirectToAction("Index");
        }
    }
}