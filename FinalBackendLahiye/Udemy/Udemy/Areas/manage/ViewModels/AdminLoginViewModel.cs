using System.ComponentModel.DataAnnotations;

namespace Udemy.Areas.manage.ViewModels
{
    public class AdminLoginViewModel
    {
        [Required]
        [MaxLength(20)]
        public string Username { get; set; }
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
