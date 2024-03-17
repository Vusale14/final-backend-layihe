using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Security.Claims;
using Udemy.DAL;
using Udemy.Models;
using Udemy.Services;
using Udemy.ViewModels;

namespace Udemy.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly UdemyContext _context;
        private readonly IEmailSender _emailSender;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, UdemyContext context,IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _emailSender = emailSender;
        }
        //User Login start
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel ViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user=await _userManager.FindByNameAsync(ViewModel.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Password or Username incorrect");
                return View();
            }

            var result= await _signInManager.PasswordSignInAsync(user, ViewModel.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Password or Username incorrect");
                return View();
            }

            TempData["Success"] = "Successfully logged in";

            return RedirectToAction("index","home");

        }
        //User Login end

        //User Register start
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel ViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewModel);
            }
            if (_context.AppUsers.Any(x => x.UserName == ViewModel.Username))
            {
                ModelState.AddModelError("Username", "Username is already exist.");
                return View(ViewModel);
            }
            if (_context.AppUsers.Any(x => x.Email == ViewModel.Email))
            {
                ModelState.AddModelError("Email", "Email is already in use.");
                return View(ViewModel);
            }
            AppUser newUser = new AppUser()
            {
                UserName = ViewModel.Username,
                Email = ViewModel.Email,
                CreationDate = DateTime.Now,
                IsAdmin = false
            };

            var result = await _userManager.CreateAsync(newUser,ViewModel.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                    return View(ViewModel);
                }
            }
            await _userManager.AddToRoleAsync(newUser, "Member");

            await _signInManager.SignInAsync(newUser, isPersistent: false);

            TempData["Success"] = "Successfully registered";

            return RedirectToAction("index","home");
        }
        //User Register end


        //User Logout start
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData["Success"] = "Successfully logged out";

            return RedirectToAction("index", "home");
        }
        //User Logout end

        //User Forget password start
        public IActionResult Forgot()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Forgot(UserForgotViewModel ViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if(!_context.AppUsers.Any(x=>x.Email==ViewModel.Email))
            {
                ModelState.AddModelError("", "There is no registered account with email");
                return View();
            }

            var user= await _userManager.FindByEmailAsync(ViewModel.Email);

            string ResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            string ResetURL = Url.Action("reset", "account", new { email = ViewModel.Email, token = ResetToken }, Request.Scheme);

            string emailMessage = await this.RenderViewToStringAsync("/Views/Shared/_ResetPartialView.cshtml",ResetURL);

            _emailSender.SendEmail(ViewModel.Email, "Reset Password", emailMessage);

            TempData["Success"] = "Reset Email send!";

            return RedirectToAction("index","home");
        }
        //User Forget password end

        //User Reset Password start
        public async Task<IActionResult> Reset(string email, string token)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token))
            {
                TempData["Error"] = "Password reset failed!";
                return RedirectToAction("login", "account");
            }

            ViewBag.Email = email;
            ViewBag.Token = token;

            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Reset(UserResetViewModel ViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(new {email=ViewModel.Email,token=ViewModel.Token});
            }
            AppUser user= await _userManager.FindByEmailAsync(ViewModel.Email);

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("index", "home");
            }

            var result = await _userManager.ResetPasswordAsync(user,ViewModel.Token,ViewModel.Password);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Password not resetted!";
                return RedirectToAction("index", "home");
            }

            TempData["Success"] = "Password resetted";
            return RedirectToAction("login","account");
        }
        //User Reset Password end

        //User Courses start
        [Authorize(Roles ="Member")]
        public IActionResult MyCourses()
        {
            AppUser user=_context.AppUsers.FirstOrDefault(x=>x.Id==User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(user == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction("index", "home");
            }
            if(!_context.AppUsersCourses.Any(x=>x.AppUserId==user.Id))
            {
                TempData["Error"] = "User dont have any courses!";
                return RedirectToAction("index", "home");
            }
            List<Course> courses = _context.Courses.Include(x=>x.AppUserCourses).Include(x=>x.CourseReviews).Include(x=>x.AppUserCourses).Where(x => x.AppUserCourses.Any(x => x.AppUserId == user.Id)).ToList();
            return View(courses);
        }

        //User Courses end

        //User CourseVideos start
        [Authorize(Roles ="Member")]
        public IActionResult CourseVideo(int courseId)
        {
            AppUser user = _context.AppUsers.Include(x=>x.AppUserCourses).ThenInclude(x=>x.Course).ThenInclude(x=>x.CourseVideos).FirstOrDefault(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(user == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction("index","home");
            }
            if(!user.AppUserCourses.Any(x=>x.CourseId==courseId))
            {
                TempData["Error"] = "Course not found!";
                return RedirectToAction("index", "home");
            }
            Course course = _context.Courses.Include(x => x.CourseVideos).FirstOrDefault(x=>x.Id==courseId);

            return View(course);
        }

    }
}
