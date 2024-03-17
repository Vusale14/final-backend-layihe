using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Udemy.Areas.manage.ViewModels;
using Udemy.DAL;
using Udemy.Models;

namespace Udemy.Areas.manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UdemyContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, UdemyContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        //Create admin
        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser admin = new AppUser()
        //    {
        //        UserName= "Admin",
        //        IsAdmin=true,
        //    };

        //    var result=await _userManager.CreateAsync(admin,"admin1245");

        //    await _userManager.AddToRoleAsync(admin, "Admin");

        //    return Json(result);


        //}
        //Admin login start
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(AdminLoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            AppUser admin = await _userManager.FindByNameAsync(viewModel.Username);
            if(admin == null)
            {
                ModelState.AddModelError("","Username or Password incorrect");
                return View();
            }
            if (!await _userManager.IsInRoleAsync(admin,"Admin"))
            {
                ModelState.AddModelError("", "Admins can only enter!");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(admin, viewModel.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }

            TempData["Success"] = "Admin logged in!";

            return RedirectToAction("index","dashboard");
            
        }

        //Admin login end

        //Admin Logout start
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Logout()
        {
            AppUser admin=_context.AppUsers.FirstOrDefault(x=>x.Id==User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (admin == null)
            {
                TempData["Error"] = "Something went wrong while logging out!";
                return RedirectToAction("index", "dashboard");
            }

            await _signInManager.SignOutAsync();


            return RedirectToAction("login", "account");
        }

        //Admin Logout end
    }
}
