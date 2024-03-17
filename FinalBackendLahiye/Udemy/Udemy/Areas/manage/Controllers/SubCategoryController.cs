using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Udemy.Areas.manage.ViewModels;
using Udemy.DAL;
using Udemy.Models;

namespace Udemy.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class SubCategoryController : Controller
    {
        private readonly UdemyContext _context;

        public SubCategoryController(UdemyContext context)
        {
            _context = context;
        }
        //Index
        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.SubCategories.Include(x=>x.Category).AsQueryable();
            if (search != null)
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            ViewBag.Search = search;

            return View(PaginatedList<SubCategory>.Create(query, page, 5));
        }
        //Index

        //Create
        public IActionResult Create()
        {
            ViewBag.Categories=_context.Categories.ToList();

            if(ViewBag.Categories.Count == 0)
            {
                TempData["Error"]= "Categories not found!";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(SubCategory subCategory)
        {
            ViewBag.Categories = _context.Categories.ToList();
            if (ViewBag.Categories.Count == 0)
            {
                TempData["Error"] = "Categories not found!";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_context.SubCategories.Any(x => x.Name == subCategory.Name))
            {
                ModelState.AddModelError("Name","SubCategory already exists!");
                return View();
            }

            if(!_context.Categories.Any(x=>x.Id == subCategory.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category not found!");
                return View();
            }

            _context.SubCategories.Add(subCategory);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        //Create

        //Delete
        public IActionResult Delete(int id)
        {
            var existSubCategory = _context.SubCategories.Find(id);

            if(existSubCategory == null)
            {
                return StatusCode(404);
            }

            _context.SubCategories.Remove(existSubCategory);
            _context.SaveChanges();

            return StatusCode(200);
        }
        //Delete

        //Edit
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            if (ViewBag.Categories.Count == 0)
            {
                TempData["Error"] = "Categories not found!";
                return RedirectToAction("Index");
            }
            var existSubCategory=_context.SubCategories.Find(id);

            if(existSubCategory == null)
            {
                TempData["Error"] = "SubCategory data not found!";
                return RedirectToAction("index");
            }

            return View(existSubCategory);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(SubCategory subCategory)
        {
            ViewBag.Categories = _context.Categories.ToList();
            if (ViewBag.Categories.Count == 0)
            {
                TempData["Error"] = "Categories not found!";
                return RedirectToAction("Index");
            }
            var existSubCategory=_context.SubCategories.Find(subCategory.Id);

            if(existSubCategory == null)
            {
                TempData["Error"] = "SubCategory data not found!";
                return RedirectToAction("index");
            }

            if(!ModelState.IsValid)
            {
                return View(existSubCategory);
            }

            if(subCategory.Name!=existSubCategory.Name&&_context.SubCategories.Any(x=>x.Name==subCategory.Name))
            {
                ModelState.AddModelError("Name","SubCategory already exists!");
                return View(existSubCategory);
            }

            if (!_context.Categories.Any(x => x.Id == subCategory.CategoryId))
            {
                ModelState.AddModelError("CategoryId","Category not found!");
                return View(existSubCategory);
            }

            existSubCategory.Name= subCategory.Name;
            existSubCategory.ExploreText= subCategory.ExploreText;
            existSubCategory.CategoryId= subCategory.CategoryId;

            _context.SaveChanges();

            TempData["Success"] = "SubCategory data edited successfully!";
            return RedirectToAction("index");
        }
        //Edit

    }
}
