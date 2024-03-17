using System.ComponentModel.DataAnnotations;

namespace Udemy.Models
{
    public class InstructorsReview
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Fullname { get; set; }
        [MaxLength(100)]
        [Required]
        public string Profession {get; set; }
        [MaxLength(500)]
        [Required]
        public string Review { get; set; }


    }
}
