using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UtilityComplaints.Core.Entities;
using UtilityComplaints.Core.Interfaces;
using UtilityComplaints.Infrastructure.Data;
using UtilityComplaints.WebUI.Models;

namespace UtilityComplaints.WebUI.Controllers
{
    [EnableCors]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IDateTimeProvider _dateTimeProvider;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, IDateTimeProvider dateTimeProvider)
        {
            _logger = logger;
            _userManager = userManager;
            _dateTimeProvider = dateTimeProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Culture(string culture, string returnUrl)
        {

            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = _dateTimeProvider.Now().AddDays(30)}
                );

            return LocalRedirect(returnUrl);
        }

        [Authorize]
        public ActionResult Chat()
        {
            return View();
        }
    }
}