using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace Udemy.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Creator { get; set; }
        [MaxLength(100)]
        public string PosterImageName { get; set; }
        [Column(TypeName = "money")]
        [Required]
        public decimal Price { get; set; }
        [Range(0,100)]
        public int DiscountPercent { get; set; }

        public double OverallRate { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
        public int TotalHours { get; set; }
        public int LectureCount { get; set; }
        [Required]
        public string ContentLevel { get; set; }

        public string Description { get; set; }

        public List<AppUserCourse> AppUserCourses { get; set; }

        public SubCategory SubCategory { get; set; }

        public List<CourseVideo> CourseVideos { get; set; }

        public List<CourseReview> CourseReviews { get; set; }
        [NotMapped]
        public IFormFile PosterImage { get; set; }
    }
}
