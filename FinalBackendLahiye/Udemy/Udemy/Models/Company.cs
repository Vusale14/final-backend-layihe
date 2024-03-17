using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Udemy.Models
{
    public class Company
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string LogoImageName { get; set; }
        [NotMapped]
        [Required]
        public IFormFile LogoImage { get; set; }
    }
}
