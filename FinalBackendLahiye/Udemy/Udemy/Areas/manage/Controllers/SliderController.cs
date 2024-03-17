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
    public class SliderController : Controller
    {
        private readonly UdemyContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(UdemyContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //Index
        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.Sliders.AsQueryable();
            if (search != null)
            {
                query = query.Where(x => x.ImageName.Contains(search));
            }

            ViewBag.Search = search;

            return View(PaginatedList<Slider>.Create(query, page, 5));
        }
        //Index

        //Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Slider slider)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            slider.ImageName=FileManager.Save(_env.WebRootPath,"uploads/SliderImages",slider.Image);

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            TempData["Success"] = "Slider created successfully!";

            return RedirectToAction("index");
        }
        //Create

        //Delete
        public IActionResult Delete(int id)
        {
            var existSlider = _context.Sliders.Find(id);

            if(existSlider == null)
            {
                return StatusCode(404);
            }

            FileManager.Delete(_env.WebRootPath,"uploads/SliderImages",existSlider.ImageName);

            _context.Sliders.Remove(existSlider);
            _context.SaveChanges();

            return StatusCode(200);

        }
        //Delete

        //Edit
        public IActionResult Edit(int id)
        {
            var existSlider=_context.Sliders.Find(id);

            if( existSlider == null)
            {
                TempData["Error"] = "Slider not found!";
                return RedirectToAction("index");
            }
            return View(existSlider);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Slider slider)
        {
            var existSlider=_context.Sliders.Find(slider.Id);
            
            if (existSlider == null)
            {
                TempData["Error"] = "Slider not found!";
                return RedirectToAction("index");
            }
            
            if(slider.Image == null)
            {
                TempData["Success"] = "Nothing changed!";
                return RedirectToAction("index");
            }

            var oldImageName = existSlider.ImageName;

            existSlider.ImageName = FileManager.Save(_env.WebRootPath, "uploads/SliderImages", slider.Image);

            _context.SaveChanges();

            FileManager.Delete(_env.WebRootPath,"uploads/SliderImages",oldImageName);

            TempData["Success"] = "Changes applied!";

            return RedirectToAction("index");

        }
        //Edit


    }
}
