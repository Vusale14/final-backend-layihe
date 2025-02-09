﻿using System.ComponentModel.DataAnnotations;

namespace Udemy.ViewModels
{
    public class UserForgotViewModel
    {
        [Required]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
