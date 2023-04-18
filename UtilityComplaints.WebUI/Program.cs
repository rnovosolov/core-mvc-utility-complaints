using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UtilityComplaints.Core.Entities;
using UtilityComplaints.Core.Interfaces;
using UtilityComplaints.Infrastructure.Data;
using UtilityComplaints.Infrastructure.Services;
using NetTopologySuite.Geometries;
using IdentityServer4;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

/*builder.Services.AddIdentityServer()
    .AddInMemoryApiScopes(Config.ApiScopes)
        .AddInMemoryClients(Config.Clients);*/


builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});




builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, 
    o => o.UseNetTopologySuite()));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews();


//builder.Services.AddScoped<IDataContext, ApplicationDbContext>();
builder.Services.AddScoped<IdentityDbContext<User>, ApplicationDbContext>();
builder.Services.AddScoped<IComplaintService, ComplaintService>();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    builder.Configuration.AddUserSecrets<Program>();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
//app.UseIdentityServer();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
