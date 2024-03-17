namespace Udemy.Models
{
    public class AppUserCartItem
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }

        public int CourseId { get; set; }

        public AppUser AppUser { get; set; }

        public Course Course { get; set; }
    }
}
