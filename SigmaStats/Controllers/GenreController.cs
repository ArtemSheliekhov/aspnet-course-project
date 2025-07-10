using Microsoft.AspNetCore.Mvc;
using SigmApi.Models;
using SigmApi.Models.Dtos;
using System.Net.Http.Json;

public class GenreController : Controller
{
    private readonly HttpClient _http;

    public GenreController(IHttpClientFactory httpClientFactory)
    {
        _http = httpClientFactory.CreateClient();
        _http.BaseAddress = new Uri("https://localhost:7230/");
    }

    public async Task<IActionResult> Index()
    {
        var genres = await _http.GetFromJsonAsync<List<GenreDto>>("api/genres");
        return View(genres);
    }

    public async Task<IActionResult> Details(int id)
    {
        var genre = await _http.GetFromJsonAsync<GenreDto>($"api/genres/{id}");
        if (genre == null) return NotFound();
        return View(genre);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GenreDto genre)
    {
        if (ModelState.IsValid)
        {
            var response = await _http.PostAsJsonAsync("api/genres", genre);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));
        }

        return View(genre);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var genre = await _http.GetFromJsonAsync<GenreDto>($"api/genres/{id}");
        if (genre == null) return NotFound();

        return View(genre);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, GenreDto genre)
    {
        if (id != genre.GenreId) return NotFound();

        if (ModelState.IsValid)
        {
            var response = await _http.PutAsJsonAsync($"api/genres/{id}", genre);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));
        }

        return View(genre);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var genre = await _http.GetFromJsonAsync<GenreDto>($"api/genres/{id}");
        if (genre == null) return NotFound();

        return View(genre);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var response = await _http.DeleteAsync($"api/genres/{id}");
        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));

        return Problem("Failed to delete the genre.");
    }
}
