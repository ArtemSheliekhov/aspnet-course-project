using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SigmApi.Models.Dtos;
using System.Net.Http.Json;
using System.Text.Json;
using static SigmApi.Controllers.GameController;

namespace SigmaStats.Controllers
{
    public class GameController : Controller
    {
        private readonly HttpClient _http;
        public GameController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7230/");

        }

        // GET: /Game
        public async Task<IActionResult> Index()
        {
            var games = await _http.GetFromJsonAsync<List<GameDto>>("api/game");
            return View(games);
        }

        // GET: /Game/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var game = await _http.GetFromJsonAsync<GameDto>($"api/game/{id}");
            if (game == null) return NotFound();
            return View(game);
        }

        // GET: /Game/Create
        public async Task<IActionResult> Create()
        {
            var companies = await _http.GetFromJsonAsync<List<CompanyDto>>("api/company");
            ViewBag.CompanyId = new SelectList(companies, "CompanyId", "Name");

            var tags = await _http.GetFromJsonAsync<List<TagDto>>("api/tags");
            ViewBag.Tags = new MultiSelectList(tags, "TagId", "Name");

            var genres = await _http.GetFromJsonAsync<List<GenreDto>>("api/genres");
            ViewBag.Genres = new MultiSelectList(genres, "GenreId", "Name");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameDto game, int[] selectedTags, int[] selectedGenres)
        {
            game.TagIds = selectedTags.ToList();
            game.GenreIds = selectedGenres.ToList();

            var response = await _http.PostAsJsonAsync("api/game", game);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var companies = await _http.GetFromJsonAsync<List<CompanyDto>>("api/company");
            ViewBag.CompanyId = new SelectList(companies, "CompanyId", "Name", game.CompanyId);

            var tags = await _http.GetFromJsonAsync<List<TagDto>>("api/tags");
            ViewBag.Tags = new MultiSelectList(tags, "TagId", "Name", selectedTags);

            var genres = await _http.GetFromJsonAsync<List<GenreDto>>("api/genres");
            ViewBag.Genres = new MultiSelectList(genres, "GenreId", "Name", selectedGenres);

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);

            return View(game);
        }

        // GET: /Game/CreateForCompany/{companyId}
        public async Task<IActionResult> CreateForCompany(int companyId)
        {
            var company = await _http.GetFromJsonAsync<CompanyDto>($"api/company/{companyId}");
    
            var gameDto = new GameDto 
            { 
                CompanyId = companyId,
                Company = company 
            };

            var tags = await _http.GetFromJsonAsync<List<TagDto>>("api/tags");
            ViewBag.Tags = new MultiSelectList(tags, "TagId", "Name");

            var genres = await _http.GetFromJsonAsync<List<GenreDto>>("api/genres");
            ViewBag.Genres = new MultiSelectList(genres, "GenreId", "Name");

            return View(gameDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForCompany(GameDto game, int[] selectedTags, int[] selectedGenres)
        {
            Console.WriteLine("=== MVC CreateForCompany called ===");
            Console.WriteLine($"Game: Name={game?.Name}, CompanyId={game?.CompanyId}, Price={game?.Price}");
            Console.WriteLine($"SelectedTags: [{string.Join(",", selectedTags ?? new int[0])}]");
            Console.WriteLine($"SelectedGenres: [{string.Join(",", selectedGenres ?? new int[0])}]");

            try
            {
                var apiDto = new GameDto
                {
                    Name = game.Name,
                    Description = game.Description,
                    Price = game.Price,
                    ReleaseDate = game.ReleaseDate,
                    CompanyId = game.CompanyId,
                    TagIds = selectedTags?.ToList() ?? new List<int>(),
                    GenreIds = selectedGenres?.ToList() ?? new List<int>()
                };

                Console.WriteLine($"Calling API at: {_http.BaseAddress}api/game");
                Console.WriteLine($"API DTO: Name={apiDto.Name}, CompanyId={apiDto.CompanyId}");

                var response = await _http.PostAsJsonAsync("api/game", apiDto);

                Console.WriteLine($"API Response Status: {response.StatusCode}");
                Console.WriteLine($"API Response Headers: {response.Headers}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success - redirecting to Index");
                    return RedirectToAction(nameof(Index));
                }

                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error Response: {error}");

                ModelState.AddModelError(string.Empty, $"Failed to create game: {error}");

                var tags = await _http.GetFromJsonAsync<List<TagDto>>("api/tags");
                ViewBag.Tags = new MultiSelectList(tags, "TagId", "Name", selectedTags);

                var genres = await _http.GetFromJsonAsync<List<GenreDto>>("api/genres");
                ViewBag.Genres = new MultiSelectList(genres, "GenreId", "Name", selectedGenres);

                return View(game);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in MVC CreateForCompany: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View(game);
            }
        }

        private async Task RepopulateViewBags(int[] selectedTags, int[] selectedGenres)
        {
            var tags = await _http.GetFromJsonAsync<List<TagDto>>("api/tags");
            ViewBag.Tags = new MultiSelectList(tags, "TagId", "Name", selectedTags);

            var genres = await _http.GetFromJsonAsync<List<GenreDto>>("api/genres");
            ViewBag.Genres = new MultiSelectList(genres, "GenreId", "Name", selectedGenres);
        }



        // GET: /Game/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var game = await _http.GetFromJsonAsync<GameDto>($"api/game/{id}");
            if (game == null) return NotFound();

            var companies = await _http.GetFromJsonAsync<List<CompanyDto>>("api/company");
            ViewBag.CompanyId = new SelectList(companies, "CompanyId", "Name", game.CompanyId);

            var tags = await _http.GetFromJsonAsync<List<TagDto>>("api/tags");
            ViewBag.Tags = new MultiSelectList(tags, "TagId", "Name", game.TagIds);

            var genres = await _http.GetFromJsonAsync<List<GenreDto>>("api/genres");
            ViewBag.Genres = new MultiSelectList(genres, "GenreId", "Name", game.GenreIds);

            return View(game);
        }


        // POST: /Game/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GameDto game)
        {
            if (!ModelState.IsValid)
            {
                var companies = await _http.GetFromJsonAsync<List<CompanyDto>>("api/company");
                ViewBag.CompanyId = new SelectList(companies, "CompanyId", "Name", game.CompanyId);

                var tags = await _http.GetFromJsonAsync<List<TagDto>>("api/tags");
                ViewBag.Tags = new MultiSelectList(tags, "TagId", "Name", game.TagIds);

                var genres = await _http.GetFromJsonAsync<List<GenreDto>>("api/genres");
                ViewBag.Genres = new MultiSelectList(genres, "GenreId", "Name", game.GenreIds);

                return View(game);
            }

            var response = await _http.PutAsJsonAsync($"api/game/{id}", game);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            // Re-populate selects in case of API failure
            var companies2 = await _http.GetFromJsonAsync<List<CompanyDto>>("api/company");
            ViewBag.CompanyId = new SelectList(companies2, "CompanyId", "Name", game.CompanyId);

            var tags2 = await _http.GetFromJsonAsync<List<TagDto>>("api/tags");
            ViewBag.Tags = new MultiSelectList(tags2, "TagId", "Name", game.TagIds);

            var genres2 = await _http.GetFromJsonAsync<List<GenreDto>>("api/genres");
            ViewBag.Genres = new MultiSelectList(genres2, "GenreId", "Name", game.GenreIds);

            return View(game);
        }


        // GET: /Game/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _http.GetFromJsonAsync<GameDto>($"api/game/{id}");
            if (game == null) return NotFound();
            return View(game);
        }

        // POST: /Game/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _http.DeleteAsync($"api/game/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return Problem("Failed to delete the game.");
        }
    }
}
