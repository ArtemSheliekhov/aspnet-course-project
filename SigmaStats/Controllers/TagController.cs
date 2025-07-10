using Microsoft.AspNetCore.Mvc;
using SigmApi.Models;
using SigmApi.Models.Dtos;
using System.Net.Http.Json;

public class TagController : Controller
{
    private readonly HttpClient _http;

    public TagController(IHttpClientFactory httpClientFactory)
    {
        _http = httpClientFactory.CreateClient();
        _http.BaseAddress = new Uri("https://localhost:7230/");
    }

    public async Task<IActionResult> Index()
    {
        var tags = await _http.GetFromJsonAsync<List<TagDto>>("api/tags");
        return View(tags);
    }

    public async Task<IActionResult> Details(int id)
    {
        var tag = await _http.GetFromJsonAsync<TagDto>($"api/tags/{id}");
        if (tag == null) return NotFound();

        return View(tag);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TagDto tag)
    {
        if (ModelState.IsValid)
        {
            var response = await _http.PostAsJsonAsync("api/tags", tag);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));
        }

        return View(tag);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var tag = await _http.GetFromJsonAsync<TagDto>($"api/tags/{id}");
        if (tag == null) return NotFound();

        return View(tag);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TagDto tag)
    {
        if (id != tag.TagId) return NotFound();

        if (ModelState.IsValid)
        {
            var response = await _http.PutAsJsonAsync($"api/tags/{id}", tag);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));
        }

        return View(tag);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var tag = await _http.GetFromJsonAsync<TagDto>($"api/tags/{id}");
        if (tag == null) return NotFound();

        return View(tag);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var response = await _http.DeleteAsync($"api/tags/{id}");
        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));

        return Problem("Failed to delete the tag.");
    }
}
