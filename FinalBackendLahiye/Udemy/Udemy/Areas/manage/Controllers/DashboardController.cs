using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Udemy.Areas.manage.ViewModels;
using Udemy.DAL;

namespace Udemy.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="Admin")]
    public class DashboardController : Controller
    {
        private readonly UdemyContext _context;

        public DashboardController(UdemyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel()
            {
                CourseCount=_context.Courses.Count(),
                UserCount=_context.Users.Count(),
                SubCategories=_context.SubCategories.Include(x=>x.Courses).ToList(),
                Categories=_context.Categories.Include(x=>x.SubCategories).ThenInclude(x=>x.Courses).ToList()
            };

            return View(dashboardViewModel);
        }
    }
}
