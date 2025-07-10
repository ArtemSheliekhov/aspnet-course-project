using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SigmApi.Models;
using SigmApi.Models.Contexts;

[Route("api/[controller]")]
[ApiController]
public class WorkerController : ControllerBase
{
    private readonly SteamContext _context;

    public WorkerController(SteamContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Worker>>> GetWorkers()
    {
        var workers = await _context.Workers
            .Select(w => new { w.WorkerId, w.Name, w.Role })
            .ToListAsync();

        return Ok(workers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Worker>> GetWorker(int id)
    {
        var worker = await _context.Workers.FindAsync(id);
        if (worker == null)
            return NotFound();

        return Ok(worker);
    }



    [HttpPost]
    public async Task<ActionResult<Worker>> CreateWorker(Worker worker)
    {
        if (await _context.Workers.AnyAsync(w => w.Name == worker.Name))
            return BadRequest("A worker with this name already exists.");

        _context.Workers.Add(worker);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWorker), new { id = worker.WorkerId }, worker);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWorker(int id, Worker worker)
    {
        if (id != worker.WorkerId)
            return BadRequest();

        _context.Entry(worker).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorker(int id)
    {
        var worker = await _context.Workers.Include(w => w.CompanyWorkers).FirstOrDefaultAsync(w => w.WorkerId == id);
        if (worker == null)
            return NotFound();

        _context.CompanyWorkers.RemoveRange(worker.CompanyWorkers);
        await _context.SaveChangesAsync();


        _context.Workers.Remove(worker);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
