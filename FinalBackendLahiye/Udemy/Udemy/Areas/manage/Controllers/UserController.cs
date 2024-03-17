using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Udemy.Areas.manage.ViewModels;
using Udemy.DAL;
using Udemy.Models;

namespace Udemy.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UdemyContext _context;

        public UserController(UdemyContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page=1,string search=null)
        {
            var query=_context.AppUsers.AsQueryable();

            if (search != null)
            {
                query = query.Where(x => x.UserName.Contains(search));
            }

            ViewBag.Search = search;

            return View(PaginatedList<AppUser>.Create(query,page,5));
        }
    }
}
