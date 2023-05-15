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
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// Load configuration information from secrets.json, if present.
builder.Configuration
    //.SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .AddJsonFile("Secrets.json", optional: true);

// Add services to the container.

builder.Services.AddAzureAppConfiguration();


// Install the Azure Key Vault and Azure Identity libraries
// using NuGet: Microsoft.Azure.KeyVault and Azure.Identity

// Create a new instance of the SecretClient using the Azure Identity library for authentication
var keyVaultUri = new Uri("https://ucsecrets.vault.azure.net/");
var credential = new DefaultAzureCredential();
var secretClient = new SecretClient(keyVaultUri, credential);

// Retrieve the secret values by name
var clientIdSecret = await secretClient.GetSecretAsync("Authentication--Google--ClientId");
var clientSecretSecret = await secretClient.GetSecretAsync("Authentication--Google--ClientSecret");
var defaultConnectionSecret = await secretClient.GetSecretAsync("ConnectionStrings--AzureDefaultConnection");

// Use the secret values to authenticate with the Google API
var clientId = clientIdSecret.Value;
var clientSecret = clientSecretSecret.Value;
var defaultConnection = defaultConnectionSecret.Value;
var connectionString = "";

try
{
    connectionString = defaultConnection.Value.ToString();
    //connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}
catch
{
    // Failed to retrieve secret from Key Vault, try retrieving from local secrets
    connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
}


/*string connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");

if (string.IsNullOrEmpty(connectionString))
{
    // Connection string not found in environment variable, use value from appsettings.json
    connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
}*/




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

        try
        {
            googleOptions.ClientId = clientId.Value.ToString();
            googleOptions.ClientSecret = clientSecret.Value.ToString();
        }
        catch
        {
            // Failed to retrieve secret from Key Vault, try retrieving from local secrets
            googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        }

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

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
        policy =>
        {
            //policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
            policy.WithOrigins("http://localhost", "https://localhost", "https://utilitycomplaints.azurewebsites.net/");
        });
});

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
builder.Services.AddHttpContextAccessor();

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
//app.UsePathBase("/wwwroot");

app.UseRouting();

app.UseAuthorization();
//app.UseIdentityServer();


var options = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);

app.UseCors("MyPolicy" /*builder =>
{
    builder.WithOrigins("http://localhost", "https://localhost", "https://utilitycomplaints.azurewebsites.net/");
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
}*/);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
