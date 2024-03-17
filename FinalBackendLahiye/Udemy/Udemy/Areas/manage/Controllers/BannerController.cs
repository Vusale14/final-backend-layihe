using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.Areas.manage.ViewModels;
using Udemy.DAL;
using Udemy.Helpers;
using Udemy.Models;

namespace Udemy.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="Admin")]
    public class BannerController : Controller
    {
        private readonly UdemyContext _context;
        private readonly IWebHostEnvironment _env;

        public BannerController(UdemyContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //Index
        public IActionResult Index(int page=1,string search=null)
        {
            var query = _context.Banners.AsQueryable();

            if(search != null)
            {
                query = query.Where(x => x.Title.Contains(search));
            }
            ViewBag.Search = search;

            return View(PaginatedList<Banner>.Create(query,page,5));
        }
        //Index

        //Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Banner banner)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(_context.Banners.Any(x=>x.Title==banner.Title))
            {
                ModelState.AddModelError("Title","Banner with this title already exists!");
                return View();
            }
            if(_context.Banners.Any(x=>x.Type==banner.Type))
            {
                ModelState.AddModelError("Type", "Banner with this type is exist!");
                return View();
            }
            if(banner.Image!=null)
            {
                banner.ImageName = FileManager.Save(_env.WebRootPath,"uploads/BannerImages",banner.Image);
            }

            _context.Banners.Add(banner);
            _context.SaveChanges();

            TempData["Success"] = "Banner data created successfully!";

            return RedirectToAction("index");
        }
        //Create

        //Delete
        public IActionResult Delete(int id)
        {
            var existBanner = _context.Banners.Find(id);

            if(existBanner==null)
            {
                return StatusCode(404);
            }

            if (existBanner.ImageName != null)
            {
                FileManager.Delete(_env.WebRootPath,"uploads/BannerImages",existBanner.ImageName);

            }

            _context.Remove(existBanner);
            _context.SaveChanges();

            return StatusCode(200);
        }
        //Delete

        //Edit
        public IActionResult Edit(int id)
        {
            var existBanner = _context.Banners.Find(id);

            if (existBanner == null)
            {
                TempData["Error"] = "Banner data not found!";
                return RedirectToAction("index");
            }

            return View(existBanner);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Banner banner)
        {
            var existBanner=_context.Banners.Find(banner.Id);
            if(existBanner==null)
            {
                TempData["Error"] = "Banner data not found!";
                return RedirectToAction("index");
            }

            if (!ModelState.IsValid)
            {
                return View(existBanner);
            }
            string oldBannerImageName = null;
            if (banner.Image!=null)
            {
                 oldBannerImageName= existBanner.ImageName;

                existBanner.ImageName=FileManager.Save(_env.WebRootPath,"uploads/BannerImages",banner.Image);
            }

            if(banner.Title!=existBanner.Title&&_context.Banners.Any(x=>x.Title==banner.Title))
            {
                ModelState.AddModelError("Title", "Banner with this title already exists!");
                return View(existBanner);
            }

            if(banner.Type!=existBanner.Type&&_context.Banners.Any(x=>x.Type==banner.Type))
            {
                ModelState.AddModelError("Type","Banner with this title already exists!");
                return View(existBanner);
            }
            existBanner.Title=banner.Title;
            existBanner.Description=banner.Description;
            existBanner.Description2= banner.Description2;
            existBanner.Description3 = banner.Description3;
            existBanner.Type=banner.Type;

            _context.SaveChanges();

            if (oldBannerImageName != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/BannerImages",oldBannerImageName);
            }
            if (oldBannerImageName != null)
            {
                FileManager.Delete(_env.WebRootPath,"uploads/BannerVideos",oldBannerImageName);
            }

            TempData["Success"] = "Banner data edited successfully!";

            return RedirectToAction("index");
        }
        //Edit
    }
}
