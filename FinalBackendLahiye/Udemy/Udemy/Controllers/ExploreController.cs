using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Udemy.DAL;
using Udemy.Models;
using Udemy.ViewModels;

namespace Udemy.Controllers
{
    public class ExploreController : Controller
    {
        private readonly UdemyContext _context;

        public ExploreController(UdemyContext context)
        {
            _context = context;
        }
        public IActionResult Index(int categoryId=0,int subcategoryId=0)
        {
            if(categoryId == 0&&subcategoryId==0)
            {
                TempData["Error"] = "None category chosen!";
                return RedirectToAction("index","home");
                
            }
            if(User.Identity.IsAuthenticated)
            {
                var existAppUser = _context.AppUsers.FirstOrDefault(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
                List<Course> existUserCourses = _context.Courses.Include(x => x.AppUserCourses).Where(x => x.AppUserCourses.Any(x => x.AppUserId == existAppUser.Id)).ToList();
                ExploreViewModel viewModel = new ExploreViewModel();
                if (categoryId != 0)
                {
                    Category category = _context.Categories.Include(x => x.SubCategories).ThenInclude(x => x.Courses).FirstOrDefault(x => x.Id == categoryId);
                    viewModel.Courses1 = _context.Courses.Include(x => x.AppUserCourses).Include(x=>x.CourseReviews).Where(x => x.SubCategory.CategoryId == categoryId).Where(x=>!existUserCourses.Contains(x)).Take(5).ToList();
                    viewModel.Courses2 = _context.Courses.Include(x => x.AppUserCourses).Include(x => x.CourseReviews).Where(x => x.SubCategory.CategoryId == categoryId).Where(x => !existUserCourses.Contains(x)).Skip(5).Take(5).ToList();
                    viewModel.AllCourses = _context.Courses.Include(x => x.AppUserCourses).Include(x => x.CourseReviews).Where(x => x.SubCategory.CategoryId == categoryId).Where(x => !existUserCourses.Contains(x)).ToList();
                    viewModel.Category = category;
                    return View(viewModel);

                }
                if (subcategoryId != 0)
                {
                    viewModel.Courses1 = _context.Courses.Include(x => x.AppUserCourses).Include(x => x.CourseReviews).Where(x => x.SubCategoryId == subcategoryId).Where(x => !existUserCourses.Contains(x)).Take(5).ToList();
                    viewModel.Courses2 = _context.Courses.Include(x => x.AppUserCourses).Include(x => x.CourseReviews).Where(x => x.SubCategoryId == subcategoryId).Where(x => !existUserCourses.Contains(x)).Skip(5).Take(5).ToList();
                    viewModel.AllCourses = _context.Courses.Include(x => x.AppUserCourses).Include(x => x.CourseReviews).Where(x => x.SubCategoryId == subcategoryId).Where(x => !existUserCourses.Contains(x)).ToList();
                    viewModel.SubCategory = _context.SubCategories.FirstOrDefault(x => x.Id == subcategoryId);
                    return View(viewModel);
                }
            }
            else
            {
                ExploreViewModel viewModel = new ExploreViewModel();
                if (categoryId != 0)
                {
                    Category category = _context.Categories.Include(x => x.SubCategories).ThenInclude(x => x.Courses).FirstOrDefault(x => x.Id == categoryId);
                    viewModel.Courses1 = _context.Courses.Include(x => x.AppUserCourses).Include(x => x.CourseReviews).Where(x => x.SubCategory.CategoryId == categoryId).Take(5).ToList();
                    viewModel.Courses2 = _context.Courses.Include(x => x.AppUserCourses).Include(x => x.CourseReviews).Where(x => x.SubCategory.CategoryId == categoryId).Skip(5).Take(5).ToList();
                    viewModel.AllCourses = _context.Courses.Include(x => x.AppUserCourses).Include(x => x.CourseReviews).Where(x => x.SubCategory.CategoryId == categoryId).ToList();
                    viewModel.Category = category;
                    return View(viewModel);

                }
                if (subcategoryId != 0)
                {
                    viewModel.Courses1 = _context.Courses.Include(x => x.AppUserCourses).Include(x => x.CourseReviews).Where(x => x.SubCategoryId == subcategoryId).Take(5).ToList();
                    viewModel.Courses2 = _context.Courses.Include(x => x.AppUserCourses).Include(x => x.CourseReviews).Where(x => x.SubCategoryId == subcategoryId).Skip(5).Take(5).ToList();
                    viewModel.AllCourses = _context.Courses.Include(x => x.AppUserCourses).Include(x => x.CourseReviews).Where(x => x.SubCategoryId == subcategoryId).ToList();
                    viewModel.SubCategory = _context.SubCategories.FirstOrDefault(x => x.Id == subcategoryId);
                    return View(viewModel);
                }
            }
            

            TempData["Error"] = "None category chosen!";
            return RedirectToAction("index", "home");
            
            
        }
    }
}
