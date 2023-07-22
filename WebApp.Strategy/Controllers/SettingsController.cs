using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Controllers
{
    [Authorize]

    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            var settings = new Settings();
            if (User.Claims.Where(x => x.Type == Settings.claimDatabaseType).FirstOrDefault() != null)
            {
                settings.databaseType = (EDatabaseType) int.Parse(User.Claims.First(x=> x.Type == Settings.claimDatabaseType).Value);
            }
            else
            {
                settings.databaseType = settings.defaultDatabaseType;
            }

            return View(settings);
        }
    }
}
