using Microsoft.AspNetCore.Mvc;

namespace SigmaStats.Controllers
{
    public class SteamGameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/games/details/{appId}")]
        public IActionResult GameDetails(int appId)
        {
            return View("~/Views/Steam/GameDetails.cshtml", appId);
        }



    }
}
