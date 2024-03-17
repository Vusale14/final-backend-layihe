using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Udemy.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string ImageName { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Image { get; set; }
    }
}
