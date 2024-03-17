using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Udemy.Areas.manage.ViewModels;
using Udemy.DAL;
using Udemy.Helpers;
using Udemy.Models;

namespace Udemy.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly UdemyContext _context;
        private readonly IWebHostEnvironment _env;

        public CourseController(UdemyContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        //Index
        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.Courses.Include(x=>x.SubCategory).AsQueryable();

            if (search != null)
            {
                query = query.Where(x => x.Name.Contains(search));
            }
            ViewBag.Search = search;

            return View(PaginatedList<Course>.Create(query, page, 5));
        }
        //Index

        //Create
        public IActionResult Create()
        {
            ViewBag.SubCategories=_context.SubCategories.ToList();

            if(ViewBag.SubCategories.Count == 0)
            {
                TempData["Error"] = "Subcategories not found!";
                return RedirectToAction("index");
            }

            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Course course)
        {
            ViewBag.SubCategories = _context.SubCategories.ToList();

            if (ViewBag.SubCategories.Count == 0)
            {
                TempData["Error"] = "Subcategories not found!";
                return RedirectToAction("index");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            if(_context.Courses.Any(x=>x.Name==course.Name))
            {
                ModelState.AddModelError("Name","Course already exists");
                return View();
            }

            if (!_context.SubCategories.Any(x => x.Id == course.SubCategoryId))
            {
                ModelState.AddModelError("SubCategoryId","SubCategory not found!");
                return View();
            }
            if (course.TotalHours == 0)
            {
                ModelState.AddModelError("TotalHours", "Total hours cannot be 0");
                return View();
            }

            if (course.LectureCount == 0)
            {
                ModelState.AddModelError("LectureCount", "Lecture count cannot be 0");
                return View();
            }

            if (course.DiscountPercent == 100)
            {
                ModelState.AddModelError("DiscountPercent","Discount Percent cannot be 100 percent!");
                return View();
            }
            if (course.Price <= 3)
            {
                ModelState.AddModelError("Price", "Price must be over than 3");
                return View();
            }

            if(course.PosterImage == null)
            {
                ModelState.AddModelError("PosterImage","Poster image is required");
                return View();
            }
            if (course.ContentLevel != "Beginner" && course.ContentLevel != "Intermidate" && course.ContentLevel != "Advanced")
            {
                ModelState.AddModelError("ContentLevel", "Level is required!");
                return View();
            }

            if (course.PosterImage!=null)
            {
                course.PosterImageName = FileManager.Save(_env.WebRootPath,"uploads/CourseImages",course.PosterImage);
            }

            

            

            _context.Courses.Add(course);
            _context.SaveChanges();

            TempData["Success"] = "Course created successfully";
            return RedirectToAction("Index");
        }
        //Create

        //Delete
        public IActionResult Delete(int id)
        {
            var existCourse = _context.Courses.Find(id);
            
            if (existCourse == null)
            {
                return StatusCode(404);
            }
            if(existCourse.PosterImageName != null)
            {
                FileManager.Delete(_env.WebRootPath,"uploads/CourseImages",existCourse.PosterImageName);
            }
            _context.Courses.Remove(existCourse);
            _context.SaveChanges();

            return StatusCode(200);
        }
        //Delete

        //Edit
        public IActionResult Edit(int id)
        {
            ViewBag.SubCategories = _context.SubCategories.ToList();

            if (ViewBag.SubCategories.Count == 0)
            {
                TempData["Error"] = "Subcategories not found!";
                return RedirectToAction("index");
            }
            var existCourse = _context.Courses.Find(id);
            if (existCourse == null)
            {
                TempData["Error"] = "Course data not found!";
                return RedirectToAction("index");
            }

            return View(existCourse);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Course course)
        {
            ViewBag.SubCategories = _context.SubCategories.ToList();

            if (ViewBag.SubCategories.Count == 0)
            {
                TempData["Error"] = "Subcategories not found!";
                return RedirectToAction("index");
            }
            var existCourse=_context.Courses.Find(course.Id);

            if (existCourse == null)
            {
                TempData["Error"]= "Course data not found";
                return RedirectToAction("index");
            }

            if (!ModelState.IsValid)
            {
                return View(existCourse);
            }

            if (course.Name!=existCourse.Name&&_context.Courses.Any(x=>x.Name==course.Name))
            {
                ModelState.AddModelError("Name","Course already exist!");
                return View(existCourse);
            }
            if (course.LectureCount == 0)
            {
                ModelState.AddModelError("LectureCount", "Lecture count cannot be 0");
                return View();
            }
            if (course.TotalHours == 0)
            {
                ModelState.AddModelError("TotalHours", "Total hours cannot be 0");
                return View();
            }

            if (course.Price <= 3)
            {
                ModelState.AddModelError("Price", "Price must be over than 3");
                return View();
            }

            if (!_context.SubCategories.Any(x => x.Id == course.SubCategoryId))
            {
                ModelState.AddModelError("SubCategoryId","SubCategory not found");
                return View(existCourse);
            }

            if (course.DiscountPercent == 100)
            {
                ModelState.AddModelError("DiscountPercent", "Discount Percent cannot be 100 percent!");
                return View();
            }
            if (course.ContentLevel != "Beginner" && course.ContentLevel != "Intermidate" && course.ContentLevel != "Advanced")
            {
                ModelState.AddModelError("ContentLevel", "Level is required!");
                return View();
            }
            string oldPosterImageName = null;
            if(course.PosterImage!=null)
            {
                oldPosterImageName=existCourse.PosterImageName;

                existCourse.PosterImageName = FileManager.Save(_env.WebRootPath, "uploads/CourseImages", course.PosterImage);
            }

            existCourse.Name=course.Name;
            existCourse.Creator=course.Creator;
            existCourse.Price=course.Price;
            existCourse.DiscountPercent=course.DiscountPercent;
            existCourse.SubCategoryId=course.SubCategoryId;
            existCourse.LectureCount=course.LectureCount;
            existCourse.TotalHours=course.TotalHours;
            existCourse.ContentLevel=course.ContentLevel;
            existCourse.Description=course.Description;

            _context.SaveChanges();

            if(oldPosterImageName!=null)
            {
                FileManager.Delete(_env.WebRootPath,"uploads/CourseImages",oldPosterImageName);
            }

            TempData["Success"] = "Course Data edited successfully";
            return RedirectToAction("index");
        }
        //Edit
    }
}
