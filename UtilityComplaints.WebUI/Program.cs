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
using IdentityServer4.Stores;
using System.Configuration;
using UtilityComplaints.WebUI.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using UtilityComplaints.WebUI.Resources;
using UtilityComplaints;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

/*builder.Services.AddIdentityServer(options =>
    options.EmitStaticAudienceClaim = true
    )
    .AddInMemoryCaching()
    .AddClientStore<InMemoryClientStore>()
    .AddResourceStore<InMemoryResourcesStore>();*/


builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        //googleOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });
//add fb/github/linkedin


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, 
    o => o.UseNetTopologySuite()));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();


builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc()
    .AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider = (t, f) => f.Create(typeof(SharedResource)))
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var supportedCultures = new List<CultureInfo>
        {
			new CultureInfo("uk"),
            new CultureInfo("en"),
            new CultureInfo("pl"),
            new CultureInfo("cs")

		};

        options.DefaultRequestCulture = new RequestCulture("uk");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;

    });

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

//builder.Services.AddScoped<IDataContext, ApplicationDbContext>();
builder.Services.AddScoped<IdentityDbContext<User>, ApplicationDbContext>();
builder.Services.AddScoped<IComplaintService, ComplaintService>();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddScoped<ISharedViewLocalizer, SharedViewLocalizer>();


builder.Services.AddSignalR();


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
app.MapHub<ChatHub>("/Chat");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
//app.UseIdentityServer();



var options = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);
//var supportedCultures = new[] { "cs", "en", "pl", "uk"};
//var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[1])
//    .AddSupportedCultures(supportedCultures)
//    .AddSupportedUICultures(supportedCultures);
//app.UseRequestLocalization(localizationOptions);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
