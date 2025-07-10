using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SigmaStats.Controllers
{
    [Route("web/dev")]
    public class DevController : Controller
    {
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

    }
}
