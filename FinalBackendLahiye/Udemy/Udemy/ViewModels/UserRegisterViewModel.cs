using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Udemy.ViewModels
{
    public class UserRegisterViewModel
    {
        [MaxLength(20)]
        [Required]
       public string Username { get; set; }
        [MaxLength(50)]
        [Required]
        [DataType(DataType.EmailAddress)]
       public string Email { get; set; }
        [Required]
        [DataType (DataType.Password)]
        [MinLength(8)]
       public string Password { get; set; }
        [MaxLength(20)]
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
       public string ConfirmPassword { get; set; }
    }
}
