using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Udemy.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(20)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [MaxLength(100)]
        public string ImageName { get; set; }

        public List<SubCategory> SubCategories { get; set;}
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
