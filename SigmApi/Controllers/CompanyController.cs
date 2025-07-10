using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SigmApi.Models;
using SigmApi.Models.Contexts;
using SigmApi.Models.Dtos;

namespace SigmApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly SteamContext _context;

        public CompanyController(SteamContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
        {
            var companies = await _context.Companies
                .Include(c => c.CompanyWorkers)
                    .ThenInclude(cw => cw.Worker)
                .Include(c => c.Games)
                .ToListAsync();

            var result = companies.Select(c => new CompanyDto
            {
                CompanyId = c.CompanyId,
                Name = c.Name,
                Headquarters = c.Headquarters,
                FoundedYear = c.FoundedYear ?? 0,
                Workers = c.CompanyWorkers.Select(cw => new WorkerDto
                {
                    WorkerId = cw.WorkerId ?? 0,
                    Name = cw.Worker?.Name ?? string.Empty,
                    Position = cw.Position,
                    JoinedDate = cw.JoinedDate
                }).ToList(),
                Games = c.Games.Select(g => new GameDto
                {
                    AppId = g.AppId,
                    Name = g.Name
                }).ToList()
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int id)
        {
            var company = await _context.Companies
                .Include(c => c.CompanyWorkers)
                    .ThenInclude(cw => cw.Worker)
                .Include(c => c.Games)
                .FirstOrDefaultAsync(c => c.CompanyId == id);

            if (company == null) return NotFound();

            var dto = new CompanyDto
            {
                CompanyId = company.CompanyId,
                Name = company.Name,
                Headquarters = company.Headquarters,
                FoundedYear = company.FoundedYear ?? 0,
                Workers = company.CompanyWorkers.Select(cw => new WorkerDto
                {
                    CompanyId = company.CompanyId,
                    WorkerId = cw.WorkerId ?? 0,
                    Name = cw.Worker?.Name ?? string.Empty,
                    Position = cw.Position,
                    JoinedDate = cw.JoinedDate
                }).ToList(),
                Games = company.Games.Select(g => new GameDto
                {
                    AppId = g.AppId,
                    Name = g.Name,
                    ReleaseDate = g.ReleaseDate ?? default,
                    Price = g.Price ?? 0,
                }).ToList()
            };

            return Ok(dto);
        }


        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyDto dto)
        {
            var company = new Company
            {
                Name = dto.Name,
                Headquarters = dto.Headquarters,
                FoundedYear = dto.FoundedYear ?? 0,
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            var createdDto = new CompanyDto
            {
                CompanyId = company.CompanyId,
                Name = company.Name,
                Headquarters = company.Headquarters,
                Workers = new(),
                Games = new()
            };

            return CreatedAtAction(nameof(GetCompany), new { id = company.CompanyId }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyDto dto)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null) return NotFound();

            company.Name = dto.Name;
            company.Headquarters = dto.Headquarters;
            company.FoundedYear = dto.FoundedYear;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies
                .Include(c => c.CompanyWorkers)
                .Include(c => c.Games)
                    .ThenInclude(g => g.Genres)
                .Include(c => c.Games)
                    .ThenInclude(g => g.Tags)
                .FirstOrDefaultAsync(c => c.CompanyId == id);

            if (company == null) return NotFound();

            foreach (var game in company.Games)
            {
                game.Genres.Clear();
                game.Tags.Clear();
            }

            _context.CompanyWorkers.RemoveRange(company.CompanyWorkers);
            _context.Games.RemoveRange(company.Games);
            _context.Companies.Remove(company);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{companyId}/assign-worker")]
        public async Task<IActionResult> AssignWorker(int companyId, WorkerDto dto)
        {
            var exists = await _context.CompanyWorkers
                .AnyAsync(cw => cw.CompanyId == companyId && cw.WorkerId == dto.WorkerId);

            if (!exists)
            {
                _context.CompanyWorkers.Add(new CompanyWorker
                {
                    CompanyId = companyId,
                    WorkerId = dto.WorkerId,
                    Position = dto.Position,
                    JoinedDate = DateOnly.FromDateTime(DateTime.Today)
                });

                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpDelete("{companyId}/remove-worker/{workerId}")]
        public async Task<IActionResult> RemoveWorker(int companyId, int workerId)
        {
            var companyWorker = await _context.CompanyWorkers
                .FirstOrDefaultAsync(cw => cw.CompanyId == companyId && cw.WorkerId == workerId);

            if (companyWorker != null)
            {
                _context.CompanyWorkers.Remove(companyWorker);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}
