
using Cinema_Booking_System.Models;
using Cinema_Booking_System.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Cinema_Booking_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryController(
            IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync(
                isTracked: false
            );

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
                return View(category);
            }

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.CommitAsync();

            TempData["Success"] = "Category added successfully";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetOneAsync(
                c => c.Id == id
            );

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
                return View(category);
            }

            _categoryRepository.Update(category);
            await _categoryRepository.CommitAsync();

            TempData["Success"] = "Category updated successfully";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetOneAsync(
                c => c.Id == id,
                new Expression<Func<Category, object>>[]
                {
                    c => c.Movies
                }
            );

            if (category == null)
                return NotFound();

            if (category.Movies.Any())
            {
                TempData["Error"] =
                    "Can't delete a category that has movies";

                return RedirectToAction("Index");
            }

            _categoryRepository.Remove(category);
            await _categoryRepository.CommitAsync();

            TempData["Success"] = "Category deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
