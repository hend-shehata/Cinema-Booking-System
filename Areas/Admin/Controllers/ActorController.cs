using Cinema_Booking_System.Data;
using Cinema_Booking_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActorController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string ActorsFolder =>
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "actors");

        private string SaveFile(IFormFile file)
        {
            Directory.CreateDirectory(ActorsFolder);
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            using (var stream = System.IO.File.Create(Path.Combine(ActorsFolder, fileName)))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }

        private void DeleteFile(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            var p = Path.Combine(ActorsFolder, fileName);
            if (System.IO.File.Exists(p)) System.IO.File.Delete(p);
        }

        public IActionResult Index()
        {
            var actors = _context.Actors.Include(a => a.Movies).ToList();
            return View(actors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Actor actor, IFormFile? img)
        {
            if (string.IsNullOrWhiteSpace(actor.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
                return View(actor);
            }

            if (img != null && img.Length > 0)
                actor.Img = SaveFile(img);

            _context.Actors.Add(actor);
            _context.SaveChanges();
            TempData["Success"] = "Actor added successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var actor = _context.Actors.Find(id);
            if (actor == null) return NotFound();
            return View(actor);
        }

        [HttpPost]
        public IActionResult Edit(Actor actor, IFormFile? img)
        {
            var old = _context.Actors.AsNoTracking().FirstOrDefault(a => a.Id == actor.Id);
            if (old == null) return NotFound();

            if (img != null && img.Length > 0)
            {
                DeleteFile(old.Img);
                actor.Img = SaveFile(img);
            }
            else
            {
                actor.Img = old.Img;
            }

            _context.Actors.Update(actor);
            _context.SaveChanges();
            TempData["Success"] = "Actor updated successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var actor = _context.Actors.FirstOrDefault(a => a.Id == id);
            if (actor == null) return NotFound();

            DeleteFile(actor.Img);
            _context.Actors.Remove(actor);
            _context.SaveChanges();
            TempData["Success"] = "Actor deleted successfully";
            return RedirectToAction("Index");
        }
    }
}