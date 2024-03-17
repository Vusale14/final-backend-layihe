using Udemy.Models;

namespace Udemy.Areas.manage.ViewModels
{
    public class DashboardViewModel
    {
        public int CourseCount { get; set; }

        public int UserCount { get; set; }

        public List<SubCategory> SubCategories { get; set; }

        public List<Category> Categories { get; set; }
    }
}
