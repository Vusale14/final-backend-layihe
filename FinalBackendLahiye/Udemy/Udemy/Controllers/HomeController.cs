using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Udemy.DAL;
using Udemy.Models;
using Udemy.ViewModels;

namespace Udemy.Controllers
{
    public class HomeController : Controller
    {
        private readonly UdemyContext _context;

        public HomeController(UdemyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewModel viewModel = new HomeViewModel()
            {
                Sliders=_context.Sliders.ToList(),
                Companies=_context.Companies.ToList(),
                Categories=_context.Categories.Include(x=>x.SubCategories).ThenInclude(x=>x.Courses).ThenInclude(x=>x.AppUserCourses).ToList(),
                SubCategories=_context.SubCategories.Take(6).ToList(),
                TopCategories=_context.Categories.Where(x=>x.Type=="Top").ToList(),
                Banner=_context.Banners.FirstOrDefault(x=>x.Type=="Home"),
                CourseReviews=_context.CourseReviews.Where(x=>x.Rate==5).Include(x=>x.AppUser).Include(x=>x.Course).Take(3).ToList(),

            };
            if (User.Identity.IsAuthenticated)
            {
                AppUser existUser=_context.AppUsers.FirstOrDefault(x=>x.Id==User.FindFirstValue(ClaimTypes.NameIdentifier));
                List<Course> existUserCourses=_context.AppUsersCourses.Include(x=>x.Course).ThenInclude(x => x.CourseReviews).Where(x=>x.AppUserId==existUser.Id).Select(x=>x.Course).ToList();
                if(existUserCourses.Any())
                {
                    viewModel.Courses = _context.Courses.Include(x=>x.CourseReviews).Include(x=>x.AppUserCourses).Where(x=>!existUserCourses.Contains(x)).ToList();
                }
                else
                {
                    viewModel.Courses=_context.Courses.Include(x => x.CourseReviews).Include(x => x.AppUserCourses).ToList();
                }
            }
            else
            {
                viewModel.Courses=_context.Courses.Include(x => x.CourseReviews).Include(x => x.AppUserCourses).ToList();
                
            }
            
            return View(viewModel);
        }

        public IActionResult AboutUs()
        {
            AboutUsViewModel viewModel = new AboutUsViewModel()
            {
                Banners=_context.Banners.Where(x=>x.Type.Contains("AboutUs")).ToList(),
                Companies=_context.Companies.ToList(),
                InstructorsReviews=_context.InstructorsReviews.ToList()
            };

            return View(viewModel);
        }
    }
}