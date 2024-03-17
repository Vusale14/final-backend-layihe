using System.ComponentModel.DataAnnotations;

namespace Udemy.Models
{
    public class CourseReview
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string AppUserId { get; set; }
        [Required]
        public int Rate { get; set; }
        [Required]
        public string ReviewText { get; set; }

        public Course Course { get; set; }

        public AppUser AppUser { get; set; }
    }
}
