using WebApp.Template.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppIdentityDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<AppIdentityDbContext>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    identityDbContext.Database.Migrate();

    if (!userManager.Users.Any())
    {
        await userManager.CreateAsync(new AppUser { UserName = "ozankazanc", Email = "ozankazanc@gmail.com", PictureUrl = "/UserPictures/signedUserIcon.png", Description = "This is a longer card with supporting text below as a natural lead-in to additional content. This content is a little bit longer." }, "Password1.");
        await userManager.CreateAsync(new AppUser { UserName = "ozankazanc2", Email = "ozankazanc2@gmail.com", PictureUrl = "/UserPictures/signedUserIcon.png", Description = "This is a longer card with supporting text below as a natural lead-in to additional content. This content is a little bit longer." }, "Password1.");
        await userManager.CreateAsync(new AppUser { UserName = "ozankazanc3", Email = "ozankazanc3@gmail.com", PictureUrl = "/UserPictures/signedUserIcon.png", Description = "This is a longer card with supporting text below as a natural lead-in to additional content. This content is a little bit longer." }, "Password1.");
        await userManager.CreateAsync(new AppUser { UserName = "ozankazanc4", Email = "ozankazanc4@gmail.com", PictureUrl = "/UserPictures/signedUserIcon.png", Description = "This is a longer card with supporting text below as a natural lead-in to additional content. This content is a little bit longer." }, "Password1.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
