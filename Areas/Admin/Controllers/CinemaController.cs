
using Cinema_Booking_System.Data;
using Cinema_Booking_System.Models;
using Cinema_Booking_System.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Cinema_Booking_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        private readonly Repository<Cinema> _cinemaRepository;

        public CinemaController(ApplicationDbContext context)
        {
            _cinemaRepository = new Repository<Cinema>(context);
        }

        private string CinemasFolder =>
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "cinemas"
            );

        private string SaveFile(IFormFile file)
        {
            Directory.CreateDirectory(CinemasFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var path = Path.Combine(CinemasFolder, fileName);

            using (var stream = System.IO.File.Create(path))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }

        private void DeleteFile(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            var path = Path.Combine(CinemasFolder, fileName);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }

        public async Task<IActionResult> Index()
        {
            var cinemas = await _cinemaRepository.GetAllAsync(
                includes: new Expression<Func<Cinema, object>>[]
                {
                    c => c.Movies
                },
                isTracked: false
            );

            return View(cinemas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cinema cinema, IFormFile? img)
        {
            if (string.IsNullOrWhiteSpace(cinema.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
                return View(cinema);
            }

            if (img != null && img.Length > 0)
            {
                cinema.Img = SaveFile(img);
            }

            await _cinemaRepository.AddAsync(cinema);
            await _cinemaRepository.CommitAsync();

            TempData["Success"] = "Cinema added successfully";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cinema = await _cinemaRepository.GetOneAsync(
                c => c.Id == id
            );

            if (cinema == null)
                return NotFound();

            return View(cinema);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cinema cinema, IFormFile? img)
        {
            var old = await _cinemaRepository.GetOneAsync(
                c => c.Id == cinema.Id,
                isTracked: false
            );

            if (old == null)
                return NotFound();

            if (img != null && img.Length > 0)
            {
                cinema.Img = SaveFile(img);
                DeleteFile(old.Img);
            }
            else
            {
                cinema.Img = old.Img;
            }

            _cinemaRepository.Update(cinema);
            await _cinemaRepository.CommitAsync();

            TempData["Success"] = "Cinema updated successfully";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var cinema = await _cinemaRepository.GetOneAsync(
                c => c.Id == id,
                new Expression<Func<Cinema, object>>[]
                {
                    c => c.Movies
                }
            );

            if (cinema == null)
                return NotFound();

            if (cinema.Movies.Any())
            {
                TempData["Error"] = "Can't delete a cinema that has movies";
                return RedirectToAction("Index");
            }

            DeleteFile(cinema.Img);

            _cinemaRepository.Remove(cinema);
            await _cinemaRepository.CommitAsync();

            TempData["Success"] = "Cinema deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
