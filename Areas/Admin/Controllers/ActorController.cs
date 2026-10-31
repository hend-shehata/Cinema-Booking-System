
using Cinema_Booking_System.Data;
using Cinema_Booking_System.Models;
using Cinema_Booking_System.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Cinema_Booking_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        private readonly Repository<Actor> _actorRepository;

        public ActorController(ApplicationDbContext context)
        {
            _actorRepository = new Repository<Actor>(context);
        }

        private string ActorsFolder =>
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "actors");

        private string SaveFile(IFormFile file)
        {
            Directory.CreateDirectory(ActorsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            using (var stream = System.IO.File.Create(
                Path.Combine(ActorsFolder, fileName)))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }

        private void DeleteFile(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            var path = Path.Combine(ActorsFolder, fileName);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }

        public async Task<IActionResult> Index()
        {
            var actors = await _actorRepository.GetAllAsync(
                includes: new Expression<Func<Actor, object>>[]
                {
                    a => a.Movies
                },
                isTracked: false
            );

            return View(actors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Actor actor, IFormFile? img)
        {
            if (string.IsNullOrWhiteSpace(actor.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
                return View(actor);
            }

            if (img != null && img.Length > 0)
                actor.Img = SaveFile(img);

            await _actorRepository.AddAsync(actor);
            await _actorRepository.CommitAsync();

            TempData["Success"] = "Actor added successfully";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var actor = await _actorRepository.GetOneAsync(
                a => a.Id == id
            );

            if (actor == null)
                return NotFound();

            return View(actor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Actor actor, IFormFile? img)
        {
            var old = await _actorRepository.GetOneAsync(
                a => a.Id == actor.Id,
                isTracked: false
            );

            if (old == null)
                return NotFound();

            if (img != null && img.Length > 0)
            {
                actor.Img = SaveFile(img);
                DeleteFile(old.Img);
            }
            else
            {
                actor.Img = old.Img;
            }

            _actorRepository.Update(actor);
            await _actorRepository.CommitAsync();

            TempData["Success"] = "Actor updated successfully";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _actorRepository.GetOneAsync(
                a => a.Id == id
            );

            if (actor == null)
                return NotFound();

            DeleteFile(actor.Img);

            _actorRepository.Remove(actor);
            await _actorRepository.CommitAsync();

            TempData["Success"] = "Actor deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
