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
    public class CompanyController : Controller
    {
        private readonly UdemyContext _context;
        private readonly IWebHostEnvironment _env;

        public CompanyController(UdemyContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //Index
        public IActionResult Index(int page=1,string search=null)
        {
            var query = _context.Companies.AsQueryable();

            if (search != null)
            {
                query = query.Where(x => x.LogoImageName.Contains(search));
            }
            ViewBag.Search = search;

            return View(PaginatedList<Company>.Create(query, page, 5));
        }
        //Index

        //Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Company company)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            company.LogoImageName = FileManager.Save(_env.WebRootPath,"uploads/CompanyImages",company.LogoImage);

            _context.Companies.Add(company);
            _context.SaveChanges();

            TempData["Success"] = "Company Data created successfully";

            return RedirectToAction("Index");

        }
        //Create

        //Delete
        public IActionResult Delete(int id)
        {
            var existCompany = _context.Companies.Find(id);

            if (existCompany == null)
            {
                return StatusCode(404);
            }

            FileManager.Delete(_env.WebRootPath,"uploads/CompanyImages",existCompany.LogoImageName);

            _context.Remove(existCompany);
            _context.SaveChanges();

            return StatusCode(200);
        }
        //Delete

        //Edit
        public IActionResult Edit(int id)
        {
            var existCompany=_context.Companies.Find(id);

            if(existCompany == null)
            {
                TempData["Error"] = "Company data not found!";
                return RedirectToAction("index");
            }

            return View(existCompany);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Company company)
        {
            var existCompany=_context.Companies.Find(company.Id);

            if(existCompany == null)
            {
                TempData["Error"] = "Company data not found!";
                return RedirectToAction("index");
            }

            if(company.LogoImage == null)
            {
                TempData["Success"] = "Nothing changed!";
                return RedirectToAction("index");
            }

            var oldLogoImageName=existCompany.LogoImageName;

            existCompany.LogoImageName=FileManager.Save(_env.WebRootPath,"uploads/CompanyImages",company.LogoImage);

            _context.SaveChanges();

            FileManager.Delete(_env.WebRootPath,"uploads/CompanyImages",oldLogoImageName);

            TempData["Success"] = "Data edited successfully!";

            return RedirectToAction("index");

        }
        //Edit
    }
}
