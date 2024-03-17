using System.ComponentModel.DataAnnotations;
using Udemy.Models;

namespace Udemy.ViewModels
{
    public class CourseReviewViewModel
    {
        public int CourseId { get; set; }

        public CourseReview CourseReview { get; set; }

    }
}
