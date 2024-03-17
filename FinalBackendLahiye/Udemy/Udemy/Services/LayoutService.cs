using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Udemy.DAL;
using Udemy.Models;

namespace Udemy.Services
{
    public class LayoutService
    {
        private readonly UdemyContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LayoutService(UdemyContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(x=>x.Key,x=>x.Value);
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.Include(x=>x.SubCategories).ToList();
        }

        public List<SubCategory> GetSubCategories()
        {
            return _context.SubCategories.ToList();
        }

        public int GetCartItemsCount()
        {
            var userId=_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return _context.AppUserCartItems.Where(x=>x.AppUserId== userId).Count();
        }
    }
}
