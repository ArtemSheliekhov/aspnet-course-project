using Microsoft.AspNetCore.Mvc;
using SigmApi.Models;
using SigmApi.Models.Dtos;
using System.Net.Http.Json;

namespace SigmaStats.Controllers
{
    public class CompanyController : Controller
    {
        private readonly HttpClient _http;

        public CompanyController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7230/");
        }

        public async Task<IActionResult> Index()
        {
            var companies = await _http.GetFromJsonAsync<List<CompanyDto>>("api/company");
            return View(companies);
        }

        public async Task<IActionResult> Details(int id)
        {
            var company = await _http.GetFromJsonAsync<CompanyDto>($"api/company/{id}");
            if (company == null) return NotFound();
            return View(company);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyDto company)
        {
            var response = await _http.PostAsJsonAsync("api/company", company);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);
            return View(company);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var company = await _http.GetFromJsonAsync<CompanyDto>($"api/company/{id}");
            if (company == null) return NotFound();
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompanyDto company)
        {
            var response = await _http.PutAsJsonAsync($"api/company/{id}", company);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(company);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var company = await _http.GetFromJsonAsync<CompanyDto>($"api/company/{id}");
            if (company == null) return NotFound();
            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _http.DeleteAsync($"api/company/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return Problem("Failed to delete the company.");
        }

        [HttpGet]
        public async Task<IActionResult> AssignWorker(int companyId)
        {
            var workers = await _http.GetFromJsonAsync<List<Worker>>("api/worker");
            ViewData["CompanyId"] = companyId;
            return View(workers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignWorker(int companyId, int workerId, string position)
        {
            var dto = new WorkerDto
            {
                WorkerId = workerId,
                CompanyId = companyId,
                Position = position
            };

            var response = await _http.PostAsJsonAsync($"api/company/{companyId}/assign-worker", dto);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Details", new { id = companyId });

            var errorContent = await response.Content.ReadAsStringAsync();
            return Problem($"Failed to assign worker. Error: {errorContent}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveWorker(int companyId, int workerId)
        {
            // Fix the URL to match your API route
            var response = await _http.DeleteAsync($"api/company/{companyId}/remove-worker/{workerId}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Details", new { id = companyId });

            var errorContent = await response.Content.ReadAsStringAsync();
            return Problem($"Failed to remove worker. Error: {errorContent}");
        }
    }
}
