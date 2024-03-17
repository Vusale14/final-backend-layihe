using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Udemy.Models
{
    public class Banner
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        [MaxLength (255)]
        public string Description2 {get; set; }
        [MaxLength(255)]
        public string Description3 { get; set; }    
        [MaxLength(100)]
        public string ImageName { get; set; }
        [MaxLength(100)]
        public string VideoName { get; set; }
        [MaxLength (20)]
        [Required]  
        public string Type { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
