using System.ComponentModel.DataAnnotations;

namespace Udemy.ViewModels
{
    public class UserResetViewModel
    {
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MinLength(8)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
