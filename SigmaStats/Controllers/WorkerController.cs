using Microsoft.AspNetCore.Mvc;
using SigmApi.Models;
using System.Net.Http.Json;

namespace SigmaStats.Controllers
{
    public class WorkerController : Controller
    {
        private readonly HttpClient _http;

        public WorkerController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7230/");
        }

        public async Task<IActionResult> Index()
        {
            var workers = await _http.GetFromJsonAsync<List<Worker>>("api/worker");
            return View(workers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var worker = await _http.GetFromJsonAsync<Worker>($"api/worker/{id}");
            if (worker == null) return NotFound();
            return View(worker);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Worker worker)
        {
            var response = await _http.PostAsJsonAsync("api/worker", worker);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);
            return View(worker);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var worker = await _http.GetFromJsonAsync<Worker>($"api/worker/{id}");
            if (worker == null) return NotFound();
            return View(worker);
        }

        [HttpGet]
        public async Task<IActionResult> SelectWorker(int companyId)
        {
            var workers = await _http.GetFromJsonAsync<List<Worker>>("api/worker");
            ViewData["CompanyId"] = companyId;
            return View(workers);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Worker worker)
        {
            if (id != worker.WorkerId) return BadRequest();

            var response = await _http.PutAsJsonAsync($"api/worker/{id}", worker);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(worker);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var worker = await _http.GetFromJsonAsync<Worker>($"api/worker/{id}");
            if (worker == null) return NotFound();
            return View(worker);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _http.DeleteAsync($"api/worker/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return Problem("Failed to delete the worker.");
        }
    }
}
