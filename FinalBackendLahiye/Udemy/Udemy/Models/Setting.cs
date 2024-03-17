using System.ComponentModel.DataAnnotations;

namespace Udemy.Models
{
    public class Setting
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
