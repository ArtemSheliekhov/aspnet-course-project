using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SigmaStats.Models;

namespace SigmaStats.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Welcome()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Stats()
    {
        return View();
    }

    public IActionResult UserStats()
    {
        return View();
    }

    public IActionResult GameInfo()
    {
        return View();
    }
    
    public IActionResult GameDetails()
    {
        return View();
    }


    public IActionResult Dashboard(string username)
    {
        ViewData["Username"] = username;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
