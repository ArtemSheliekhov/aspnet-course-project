using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SigmApi.Helpers;           
using SigmApi.Models;
using SigmApi.Models.Contexts;
using SigmApi.Models.Dtos;       

namespace SigmApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly SteamContext _context;

        public TagsController(SteamContext context)
        {
            _context = context;
        }

        // GET: api/Tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetTags()
        {
            var tags = await _context.Tags.ToListAsync();
            return tags.Select(t => (TagDto)t).ToList();
        }

        // GET: api/Tags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TagDto>> GetTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
                return NotFound();

            return (TagDto)tag;
        }

        // POST: api/Tags
        [HttpPost]
        public async Task<ActionResult<TagDto>> CreateTag(TagDto tagDto)
        {
            var tag = new Tag
            {
                Name = tagDto.Name
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return Ok((TagDto)tag);
        }

        // PUT: api/Tags/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, TagDto tagDto)
        {
            if (id != tagDto.TagId)
                return BadRequest();

            var tagDb = await _context.Tags.FindAsync(id);
            if (tagDb == null)
                return NotFound();

            tagDb.CopyProperties(tagDto);
            _context.Entry(tagDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(id))
                    return NotFound();
                else
                    throw;
            }

            return Ok();
        }

        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return NotFound();

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.TagId == id);
        }
    }
}
