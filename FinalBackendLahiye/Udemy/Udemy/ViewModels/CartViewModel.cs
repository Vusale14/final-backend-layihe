using Udemy.Models;

namespace Udemy.ViewModels
{
    public class CartViewModel
    {
        public List<Course> UserCourses { get; set; }

        public List<Course> MightLikeCourses { get; set; }

        public decimal OldTotalPrice { get; set; }

        public decimal NewTotalPrice { get; set; }

        public decimal TotalDiscountPercent { get; set; }
    }
}
