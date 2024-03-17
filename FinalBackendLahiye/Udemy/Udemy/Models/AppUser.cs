using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Udemy.Models
{
    public class AppUser:IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set;}
        [Required]
        public bool IsAdmin { get; set;}
        [Required]
        public DateTime CreationDate { get; set;}

        public List<AppUserCourse> AppUserCourses { get; set;}=new List<AppUserCourse>();

        public List<AppUserCartItem> AppUserCartItems { get; set;} =new List<AppUserCartItem>();

        public List<CourseReview> CoursesReviews { get; set;} =new List<CourseReview>();


    }
}
