using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.Areas.manage.ViewModels;
using Udemy.DAL;
using Udemy.Models;

namespace Udemy.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        private readonly UdemyContext _context;

        public SettingController(UdemyContext context)
        {
            _context = context;
        }
        //Index
        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.Settings.AsQueryable();

            if (search != null)
            {
                query = query.Where(x => x.Key.Contains(search));
            }
            ViewBag.Search = search;

            return View(PaginatedList<Setting>.Create(query, page, 5));
        }
        //Index

        //Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Setting setting)
        {
            if(!ModelState.IsValid)
            {
                return View(setting);
            }

            if (_context.Settings.Any(x => x.Key == setting.Key))
            {
                ModelState.AddModelError("Key","Setting data already exist!");
                return View(setting);
            }

            _context.Settings.Add(setting);
            _context.SaveChanges();

            TempData["Success"] = "Setting successfully created!";

            return RedirectToAction("index");
        }

        //Create

        //Delete
        public IActionResult Delete(string key)
        {
            var existSetting = _context.Settings.FirstOrDefault(x => x.Key == key);

            if(existSetting == null)
            {
                return StatusCode(404);
            }

            _context.Settings.Remove(existSetting);
            _context.SaveChanges();

            return StatusCode(200);

        }
        //Delete

        //Edit
        public IActionResult Edit(string key)
        {
            var existSetting = _context.Settings.Find(key);

            if (existSetting == null)
            {
                TempData["Error"] = "Setting data not found!";
                return RedirectToAction("index");   
            }

            return View(existSetting);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Setting setting)
        {
            if(!ModelState.IsValid)
            {
                return View(setting);
            }
            var existSetting = _context.Settings.Find(setting.Key);
            if( existSetting == null)
            {
                TempData["Error"] = "Setting data not found!";
                return RedirectToAction("index");
            }

            existSetting.Key = setting.Key;
            existSetting.Value = setting.Value;

            _context.SaveChanges();

            TempData["Success"] = "Data edited successfully!";
            return RedirectToAction("index");

        }
        //Edit
    }
}
