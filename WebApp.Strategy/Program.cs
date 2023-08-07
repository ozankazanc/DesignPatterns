using WebApp.Strategy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Strategy.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddScoped<IProductRepository>(opt =>
//{
//    var httpContextAccessor = opt.GetRequiredService<HttpContextAccessor>();
//    var claim = httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == Settings.claimDatabaseType).FirstOrDefault();

//    var context = opt.GetRequiredService<AppIdentityDbContext>();
//    if (claim == null)
//        return new ProductRepositoryFromSqlServer(context);

//    var databaseType =
    
//});


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
        await userManager.CreateAsync(new AppUser { UserName = "ozankazanc", Email = "ozankazanc@gmail.com" }, "Password1.");
        await userManager.CreateAsync(new AppUser { UserName = "ozankazanc2", Email = "ozankazanc2@gmail.com" }, "Password1.");
        await userManager.CreateAsync(new AppUser { UserName = "ozankazanc3", Email = "ozankazanc3@gmail.com" }, "Password1.");
        await userManager.CreateAsync(new AppUser { UserName = "ozankazanc4", Email = "ozankazanc4@gmail.com" }, "Password1.");
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
