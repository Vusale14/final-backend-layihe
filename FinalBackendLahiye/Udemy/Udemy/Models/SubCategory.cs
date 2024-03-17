using System.ComponentModel.DataAnnotations;

namespace Udemy.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        [MaxLength(20)]
        [Required]
        public string Name { get; set; }
        [Required] 
        public int CategoryId { get; set; }

        public string ExploreText { get; set; }

        public string FAQText { get;set; }

        public List<Course> Courses { get; set; }=new List<Course>();

        public Category Category { get; set; }
    }
}
