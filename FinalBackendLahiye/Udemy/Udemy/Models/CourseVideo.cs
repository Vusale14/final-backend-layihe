namespace Udemy.Models
{
    public class CourseVideo
    {
        public int Id { get; set; }

        public string VideoName { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
