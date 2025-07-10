using Microsoft.AspNetCore.Mvc;
using SigmApi.Models.Dtos;
using System.Net.Http.Json;

namespace SigmaStats.Controllers
{
    public class SteamController : Controller
    {
        private readonly HttpClient _http;

        public SteamController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7230/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            if (ModelState.IsValid)
            {
                var response = await _http.PostAsJsonAsync("api/steam", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                    return RedirectToAction("Stats", "Home");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var rawContent = await response.Content.ReadAsStringAsync();
                    string errorMessage = "Login failed";

                    if (!string.IsNullOrWhiteSpace(rawContent))
                    {
                        try
                        {
                            var error = System.Text.Json.JsonSerializer.Deserialize<ErrorResponseDto>(rawContent);
                            if (!string.IsNullOrWhiteSpace(error?.Message))
                                errorMessage = error.Message;
                        }
                        catch (System.Text.Json.JsonException)
                        {
                            errorMessage = rawContent;
                        }
                    }

                    ModelState.AddModelError(string.Empty, errorMessage);
                }

            }



            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string sessionId)
        {
            var response = await _http.PostAsJsonAsync($"api/steam/logout/{sessionId}", new { });

            if (response.IsSuccessStatusCode)
            {
                // Clear session/token
                return RedirectToAction("Index", "Home");
            }

            return Problem("Logout failed");
        }

    }
}