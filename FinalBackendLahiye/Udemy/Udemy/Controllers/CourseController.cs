using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Udemy.DAL;
using Udemy.Models;
using Udemy.Services;
using Udemy.ViewModels;

namespace Udemy.Controllers
{
    public class CourseController : Controller
    {
        private readonly UdemyContext _context;

        public CourseController(UdemyContext context)
        {
            _context = context;
        }
        public IActionResult Detail(int id)
        {
            var course=_context.Courses
                .Include(x=>x.SubCategory)
                .Include(x=>x.CourseReviews).ThenInclude(x=>x.AppUser)
                .Include(x=>x.AppUserCourses)
                .FirstOrDefault(x=>x.Id == id);
            if (course == null)
            {
                TempData["Error"] = "Course not found!";
                return RedirectToAction("index","home");
            }

            return View(course);
        }
        [Authorize(Roles ="Member")]
        public IActionResult AddToCart(int id)
        {
            var existUser = _context.AppUsers.Include(x=>x.AppUserCartItems).Include(x=>x.AppUserCourses).FirstOrDefault(x=>x.Id==User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (existUser == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction("index", "home");
            }
            var existCourse = _context.Courses.Find(id);
            if (existCourse == null)
            {
                TempData["Error"] = "Course not found!";
                return RedirectToAction("index","home");
            }
            if(existUser.AppUserCartItems.Any(x=>x.CourseId == existCourse.Id))
            {
                TempData["Error"] = "Item is already in cart!";
                return RedirectToAction("detail",new {id=existCourse.Id});
            }
            if (existUser.AppUserCourses.Any(x => x.CourseId == existCourse.Id))
            {
                TempData["Error"] = "You already bought this course!";
                return RedirectToAction("index", "home");
            }
            AppUserCartItem cartItem = new AppUserCartItem()
            {
                AppUserId=existUser.Id,
                CourseId=existCourse.Id,
            };

            _context.AppUserCartItems.Add(cartItem);
            _context.SaveChanges();

            TempData["Success"] = "Course successfully added to cart!";

            return RedirectToAction("detail", new { id = existCourse.Id });
            

        }
        [Authorize(Roles ="Member")]
        public IActionResult RemoveCartItem(int id)
        {
            var existUser = _context.AppUsers.Include(x=>x.AppUserCartItems).FirstOrDefault(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (existUser == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction("index","home");
            }

            if (!existUser.AppUserCartItems.Any(x => x.CourseId == id))
            {
                TempData["Error"] = "You dont have this course in your cart!";
                return RedirectToAction("index","home");
            }

            var existCartItem = _context.AppUserCartItems.FirstOrDefault(x => x.AppUserId == existUser.Id && x.CourseId == id);

            _context.AppUserCartItems.Remove(existCartItem);
            _context.SaveChanges();

            return RedirectToAction("cart");
        }
        [Authorize(Roles ="Member")]
        public IActionResult Checkout()
        {
            var existAppUser = _context.AppUsers.Include(x=>x.AppUserCartItems).ThenInclude(x=>x.Course).Include(x=>x.AppUserCourses).ThenInclude(x=>x.Course).FirstOrDefault(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(existAppUser == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction("index", "home");
            }
            var existUserCartItems=existAppUser.AppUserCartItems.ToList();
            var existUserCartCourses = existUserCartItems.Select(x => x.Course).ToList();
            var existUserCourses = existAppUser.AppUserCourses.Select(x => x.Course).ToList();
            if (!existUserCartItems.Any())
            {
                TempData["Error"] = "There not item in cart!";
                return RedirectToAction("index","home");
            }
            foreach (var crs in existUserCourses)
            {
                if (existUserCartCourses.Contains(crs))
                {
                    TempData["Error"] = "You have already bought this course!";
                    return RedirectToAction("index", "home");
                }
            }
            foreach(var item in existUserCartItems)
            {
                AppUserCourse addedCourse = new AppUserCourse()
                {
                    AppUserId = existAppUser.Id,
                    CourseId = item.CourseId,
                };
                _context.AppUsersCourses.Add(addedCourse);
            }
            var deletedCartItems=_context.AppUserCartItems.Where(x=>x.AppUserId==existAppUser.Id);
            _context.AppUserCartItems.RemoveRange(deletedCartItems);
            _context.SaveChanges();

            TempData["Success"] = "Courses bought successfully!";
            return RedirectToAction("index","home");
        }
        [Authorize(Roles ="Member")]
        public IActionResult Cart()
        {
            var existUser = _context.AppUsers.Include(x=>x.AppUserCartItems).FirstOrDefault(x=>x.Id==User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (existUser == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction("index","home");
            }
            if(existUser.AppUserCartItems.Count() == 0)
            {
                TempData["Error"] = "You dont have cart items!";
                return RedirectToAction("index","home");
            }
            List<SubCategory> userCoursesSubCategories = _context.AppUserCartItems.Include(x=>x.Course).ThenInclude(x=>x.SubCategory).Include(x=>x.Course).ThenInclude(x=>x.AppUserCourses).Where(x => x.AppUserId == existUser.Id).Select(x=>x.Course.SubCategory).ToList();
            var UserCourses = _context.AppUserCartItems.Include(x => x.Course).ThenInclude(x=>x.AppUserCourses).Include(x=>x.Course).ThenInclude(x=>x.CourseReviews).Where(x => x.AppUserId == existUser.Id).Select(x => x.Course).ToList();
            var MightLikeCourses = _context.Courses.Include(x => x.AppUserCourses).Include(x=>x.CourseReviews).Where(x => userCoursesSubCategories.Contains(x.SubCategory)).Where(x=>!UserCourses.Contains(x)).Take(10).ToList();
            decimal NewTotalPrice = 0;
            decimal OldTotalPrice = 0;
            foreach (var crs in UserCourses)
            {
                if (crs.DiscountPercent > 0)
                {
                    NewTotalPrice+=crs.Price * (100 - crs.DiscountPercent) / 100;
                }
                else
                {
                    NewTotalPrice+=crs.Price;
                }
            }
            foreach (var crs in UserCourses)
            {
                OldTotalPrice+=crs.Price;
            }
            decimal TotalDiscountPercent=100-((NewTotalPrice/OldTotalPrice)*100);
            CartViewModel cartViewModel = new CartViewModel()
            {
                OldTotalPrice = OldTotalPrice,
                NewTotalPrice = NewTotalPrice,
                TotalDiscountPercent = TotalDiscountPercent,
                MightLikeCourses= MightLikeCourses,
                UserCourses= UserCourses
                
            };
            

            return View(cartViewModel);
        }

        [Authorize(Roles ="Member")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddComment(CourseReviewViewModel viewModel)
        {
            var existCourse = _context.Courses.Include(x => x.CourseReviews).FirstOrDefault(x => x.Id == viewModel.CourseId);
            if (existCourse == null)
            {
                TempData["Error"] = "Course not found";
                return RedirectToAction("detail",new {id=existCourse.Id});
            }
            var existUser = _context.AppUsers.Find( User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (existUser == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction("index", "home");
            }
            if(!ModelState.IsValid)
            {
                TempData["Error"] = "Comment not posted due invalid comment!";
                return RedirectToAction("detail", new { id = existCourse.Id });
            }
            
            
            
            viewModel.CourseReview.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            viewModel.CourseReview.CourseId=existCourse.Id;

            _context.CourseReviews.Add(viewModel.CourseReview);
            _context.SaveChanges();

            existCourse.OverallRate = existCourse.CourseReviews.Sum(x => x.Rate) / existCourse.CourseReviews.Count();
            _context.SaveChanges();

            TempData["Success"] = "Comment posted!";

            return RedirectToAction("detail",new {id=existCourse.Id});
        }
    }
}
