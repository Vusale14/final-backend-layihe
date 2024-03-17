using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.Areas.manage.ViewModels;
using Udemy.DAL;
using Udemy.Helpers;
using Udemy.Models;

namespace Udemy.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly UdemyContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(UdemyContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //Index
        public IActionResult Index(int page=1,string search=null)
        {
            var query = _context.Categories.AsQueryable();
            if (search != null)
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            ViewBag.Search = search;
            
            return View(PaginatedList<Category>.Create(query,page,5));
        }
        //Index

        //Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(_context.Categories.Any(x=>x.Name==category.Name))
            {
                ModelState.AddModelError("Name","Category already exists!");
                return View();
            }
            if(category.Image!=null)
            {
                category.ImageName = FileManager.Save(_env.WebRootPath, "uploads/CategoryImages", category.Image);
            }

            _context.Categories.Add(category);
            _context.SaveChanges();

            TempData["Success"] = "Category data created successfully!";

            return RedirectToAction("index");
        }
        //Create

        //Delete
        public IActionResult Delete(int id)
        {
            var existCategory = _context.Categories.Find(id);

            if (existCategory == null)
            {
                return StatusCode(404);
            }

            if (existCategory.ImageName != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/CategoryImages",existCategory.ImageName);
            }

            _context.Remove(existCategory);
            _context.SaveChanges();

            return StatusCode(200);
        }
        //Delete

        //Edit
        public IActionResult Edit(int id)
        {
            var existCategory=_context.Categories.Find(id);

            if(existCategory == null)
            {
                TempData["Error"] = "Category data not found!";
                return RedirectToAction("index");
            }

            return View(existCategory);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Category category)
        {
            var existCategory = _context.Categories.Find(category.Id);

            if(existCategory == null)
            {
                TempData["Error"] = "Category data not found!";
                return RedirectToAction("index");
            }
            if(!ModelState.IsValid)
            {
                return View(existCategory);
            }
            if(category.Name!=existCategory.Name&&_context.Categories.Any(x=>x.Name==category.Name))
            {
                ModelState.AddModelError("Name","Category already exists");
                return View(existCategory);
            }

            string oldCategoryImageName = null;
            if (category.Image != null)
            {
                oldCategoryImageName = existCategory.ImageName;

                existCategory.ImageName=FileManager.Save(_env.WebRootPath, "uploads/CategoryImages", category.Image);

            }

            existCategory.Name=category.Name;
            existCategory.Type=category.Type;

            _context.SaveChanges();

            if(oldCategoryImageName!=null)
            {
                FileManager.Delete(_env.WebRootPath,"uploads/CategoryImages",oldCategoryImageName);
            }

            TempData["Success"] = "Category Data edited successfully";

            return RedirectToAction("index");
        }

        //Edit


    }
}
