using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Udemy.DAL;
using Udemy.Models;
using Udemy.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//Sql database start
builder.Services.AddDbContext<UdemyContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));

});
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 8;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
}).AddDefaultTokenProviders().AddEntityFrameworkStores<UdemyContext>();
//Sql database end

//Services start
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<LayoutService>();
//Services end
//Redirect to login
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = options.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.HttpContext.Request.Path.Value.StartsWith("/manage"))
        {
            context.Response.Redirect("/manage/account/login");
        }
        else
        {
            context.Response.Redirect("/account/login");
        }

        return Task.CompletedTask;
    };
});
//Redirect to login
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
