using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Udemy.Models;

namespace Udemy.DAL
{
    public class UdemyContext :IdentityDbContext
    {
        public UdemyContext(DbContextOptions<UdemyContext> options):base(options) 
        {
            
        }
        
        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<Slider> Sliders { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<AppUserCourse> AppUsersCourses { get; set; }

        public  DbSet<Banner> Banners { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<InstructorsReview> InstructorsReviews { get; set; }

        public DbSet<CourseVideo> CourseVideos { get; set; }

        public DbSet<AppUserCartItem> AppUserCartItems { get; set; }

        public DbSet<CourseReview> CourseReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Setting>().HasKey(x => x.Key);
            builder.Entity<AppUserCourse>().HasKey(x=>new {x.AppUserId,x.CourseId});

            base.OnModelCreating(builder);
        }

    }
}
