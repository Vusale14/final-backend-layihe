using System.ComponentModel.DataAnnotations;

namespace Udemy.Models
{
    public class AppUserCourse
    {
        [Required]
        public int CourseId { get; set; }
        [Required]
        public string AppUserId { get; set; }

        public Course Course { get; set; }

        public AppUser User { get; set; }
    }
}
