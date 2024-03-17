using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.Areas.manage.ViewModels;
using Udemy.DAL;
using Udemy.Models;

namespace Udemy.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class InstructorReviewController : Controller
    {
        private readonly UdemyContext _context;

        public InstructorReviewController(UdemyContext context)
        {
            _context = context;
        }
        //Index
        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.InstructorsReviews.AsQueryable();

            if (search != null)
            {
                query = query.Where(x => x.Fullname.Contains(search));
            }
            ViewBag.Search = search;

            return View(PaginatedList<InstructorsReview>.Create(query, page, 5));
        }
        //Index

        //Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(InstructorsReview instructorsReview)
        {
            if(!ModelState.IsValid)
            {
                return View(instructorsReview);
            }

            if (_context.InstructorsReviews.Any(x=>x.Fullname==instructorsReview.Fullname))
            {
                ModelState.AddModelError("Fullname","Instructor review already exist!");
                return View(instructorsReview);
            }
            _context.InstructorsReviews.Add(instructorsReview);
            _context.SaveChanges();

            TempData["Success"] = "Instructor Review created successfully";
            return RedirectToAction("index");
        }
        //Create

        //Delete
        public IActionResult Delete(int id)
        {
            var instructorReview=_context.InstructorsReviews.Find(id);

            if (instructorReview == null)
            {
                return StatusCode(404);
            }

            _context.Remove(instructorReview);
            _context.SaveChanges();

            return StatusCode(200);
        }
        //Delete

        //Edit
        public IActionResult Edit(int id)
        {
            var instructorReview = _context.InstructorsReviews.Find(id);

            if(instructorReview == null)
            {
                TempData["Error"] = "Instructor Review not found!";
                return RedirectToAction("index");
            }

            return View(instructorReview);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(InstructorsReview instructorsReview)
        {
            var existInstructorReview = _context.InstructorsReviews.Find(instructorsReview.Id);
            if(existInstructorReview == null)
            {
                TempData["Error"] = "Instructor Review not found!";
                return RedirectToAction("index");
            }
            if(!ModelState.IsValid)
            {
                return View(existInstructorReview);
            }

            if(instructorsReview.Fullname != existInstructorReview.Fullname && _context.InstructorsReviews.Any(x => x.Fullname == instructorsReview.Fullname))
            {
                ModelState.AddModelError("Fullname", "Instructor Review already exist!");
                return View(existInstructorReview);
            }
            existInstructorReview.Fullname= instructorsReview.Fullname;
            existInstructorReview.Profession= instructorsReview.Profession;
            existInstructorReview.Review=instructorsReview.Review;

            _context.SaveChanges();

            TempData["Success"] = "Data edited successfully!";

            return RedirectToAction("index");
        }
        //Edit
    }
}
