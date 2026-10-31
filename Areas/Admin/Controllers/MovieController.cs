using Cinema_Booking_System.Data;
using Cinema_Booking_System.Models;
using Cinema_Booking_System.ViewModels;
using Cinema_Booking_System.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace Cinema_Booking_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly Repository<Movie> _movieRepository;
        private readonly Repository<Category> _categoryRepository;
        private readonly Repository<Cinema> _cinemaRepository;
        private readonly Repository<Actor> _actorRepository;

        public MovieController(ApplicationDbContext context)
        {
            _movieRepository = new Repository<Movie>(context);
            _categoryRepository = new Repository<Category>(context);
            _cinemaRepository = new Repository<Cinema>(context);
            _actorRepository = new Repository<Actor>(context);
        }

        private string MoviesFolder =>
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "movies"
            );

        private string SaveFile(IFormFile file)
        {
            Directory.CreateDirectory(MoviesFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            using (var stream = System.IO.File.Create(
                Path.Combine(MoviesFolder, fileName)))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }

        private void DeleteFile(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            var path = Path.Combine(MoviesFolder, fileName);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }

        private async Task<MovieFormViewModel> BuildForm(Movie? movie = null)
        {
            var categories = await _categoryRepository.GetAllAsync(
                isTracked: false
            );

            var cinemas = await _cinemaRepository.GetAllAsync(
                isTracked: false
            );

            var actors = await _actorRepository.GetAllAsync(
                isTracked: false
            );

            var vm = new MovieFormViewModel
            {
                Movie = movie ?? new Movie
                {
                    DateTime = DateTime.Now.AddDays(1)
                },

                Categories = categories.Select(c =>
                    new SelectListItem(c.Name, c.Id.ToString())
                ).ToList(),

                Cinemas = cinemas.Select(c =>
                    new SelectListItem(c.Name, c.Id.ToString())
                ).ToList(),

                Actors = actors.Select(a =>
                    new SelectListItem(a.Name, a.Id.ToString())
                ).ToList()
            };

            if (movie != null)
            {
                vm.SelectedActorIds = movie.Actors
                    .Select(a => a.Id)
                    .ToList();
            }

            return vm;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieRepository.GetAllAsync(
                includes: new Expression<Func<Movie, object>>[]
                {
                    m => m.Category,
                    m => m.Cinema
                },
                isTracked: false
            );

            return View(movies);
        }

        public async Task<IActionResult> Create()
        {
            return View(await BuildForm());
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            MovieFormViewModel vm,
            IFormFile? mainImg,
            List<IFormFile>? subImages)
        {
            var movie = vm.Movie;

            if (string.IsNullOrWhiteSpace(movie.Name) ||
                mainImg == null || mainImg.Length == 0)
            {
                ModelState.AddModelError(
                    "",
                    "Name and main image are required"
                );

                var again = await BuildForm(movie);
                again.SelectedActorIds = vm.SelectedActorIds;

                return View(again);
            }

            movie.MainImg = SaveFile(mainImg);

            if (subImages != null)
            {
                foreach (var file in subImages.Where(f => f.Length > 0))
                {
                    movie.SubImages.Add(
                        new MovieSubImage
                        {
                            Img = SaveFile(file)
                        }
                    );
                }
            }

            movie.Actors = (await _actorRepository.GetAllAsync(
                a => vm.SelectedActorIds.Contains(a.Id)
            )).ToList();

            await _movieRepository.AddAsync(movie);
            await _movieRepository.CommitAsync();

            TempData["Success"] = "Movie added successfully";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _movieRepository.GetOneAsync(
                m => m.Id == id,
                new Expression<Func<Movie, object>>[]
                {
                    m => m.Actors,
                    m => m.SubImages
                }
            );

            if (movie == null)
                return NotFound();

            return View(await BuildForm(movie));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            MovieFormViewModel vm,
            IFormFile? mainImg,
            List<IFormFile>? subImages)
        {
            var form = vm.Movie;

            var movie = await _movieRepository.GetOneAsync(
                m => m.Id == form.Id,
                new Expression<Func<Movie, object>>[]
                {
                    m => m.Actors,
                    m => m.SubImages
                }
            );

            if (movie == null)
                return NotFound();

            movie.Name = form.Name;
            movie.Des = form.Des;
            movie.Price = form.Price;
            movie.Status = form.Status;
            movie.DateTime = form.DateTime;
            movie.CategoryId = form.CategoryId;
            movie.CinemaId = form.CinemaId;

            string? oldMainImg = null;

            if (mainImg != null && mainImg.Length > 0)
            {
                oldMainImg = movie.MainImg;
                movie.MainImg = SaveFile(mainImg);
            }

            if (subImages != null)
            {
                foreach (var file in subImages.Where(f => f.Length > 0))
                {
                    movie.SubImages.Add(
                        new MovieSubImage
                        {
                            Img = SaveFile(file)
                        }
                    );
                }
            }

            movie.Actors.Clear();

            var actors = await _actorRepository.GetAllAsync(
                a => vm.SelectedActorIds.Contains(a.Id)
            );

            foreach (var actor in actors)
            {
                movie.Actors.Add(actor);
            }

            await _movieRepository.CommitAsync();

            if (oldMainImg != null)
                DeleteFile(oldMainImg);

            TempData["Success"] = "Movie updated successfully";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieRepository.GetOneAsync(
                m => m.Id == id,
                new Expression<Func<Movie, object>>[]
                {
                    m => m.SubImages
                }
            );

            if (movie == null)
                return NotFound();

            var mainImg = movie.MainImg;

            var subImageNames = movie.SubImages
                .Select(s => s.Img)
                .ToList();

            _movieRepository.Remove(movie);

            var result = await _movieRepository.CommitAsync();

            if (result < 0)
            {
                TempData["Error"] = "Failed to delete movie";
                return RedirectToAction("Index");
            }

            DeleteFile(mainImg);

            foreach (var image in subImageNames)
            {
                DeleteFile(image);
            }

            TempData["Success"] = "Movie deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
