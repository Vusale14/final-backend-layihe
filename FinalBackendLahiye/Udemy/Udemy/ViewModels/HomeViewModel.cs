using Udemy.Models;

namespace Udemy.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }=new List<Slider>();

        public List<Company> Companies { get; set; }=new List<Company>();

        public List<Course> Courses { get; set; } = new List<Course>();

        public List<SubCategory> SubCategories { get; set; } =new List<SubCategory>();

        public List<Category> Categories { get; set; } = new List<Category>();

        public List<Category> TopCategories { get; set; } = new List<Category>();

        public List<CourseReview> CourseReviews { get; set; }

        public Banner Banner { get; set; }

    }
}
