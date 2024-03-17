using System.ComponentModel.DataAnnotations;

namespace Udemy.ViewModels
{
    public class UserLoginViewModel
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
