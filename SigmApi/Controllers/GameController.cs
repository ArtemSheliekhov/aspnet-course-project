using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SigmApi.Models;
using SigmApi.Models.Contexts;
using SigmApi.Models.Dtos;

namespace SigmApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly SteamContext _context;

        public GameController(SteamContext context)
        {
            _context = context;
        }

        // GET: api/Games/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames()
        {
            var games = await _context.Games
                .Include(g => g.Company)
                .Include(g => g.Tags)
                .Include(g => g.Genres)
                .ToListAsync();

            var dtos = games.Select(g => new GameDto
            {
                AppId = g.AppId,
                Name = g.Name,
                Description = g.Description,
                ReleaseDate = g.ReleaseDate.HasValue ? g.ReleaseDate.Value : default,
                Price = g.Price.HasValue ? g.Price.Value : default, 
                Company = g.Company == null ? null : new CompanyDto
                {
                    CompanyId = g.Company.CompanyId,
                    Name = g.Company.Name
                },
                Tags = g.Tags.Select(t => new TagDto
                {
                    TagId = t.TagId,
                    Name = t.Name
                }).ToList(),
                Genres = g.Genres.Select(ge => new GenreDto
                {
                    GenreId = ge.GenreId,
                    Name = ge.Name
                }).ToList()
            }).ToList();

            return Ok(dtos);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _context.Games
                .Include(g => g.Company)
                .Include(g => g.Tags)
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(g => g.AppId == id);

            if (game == null)
                return NotFound();

            var dto = new GameDto
            {
                AppId = game.AppId,
                Name = game.Name,
                Description = game.Description,
                ReleaseDate = game.ReleaseDate.HasValue ? game.ReleaseDate.Value : default,
                Price = game.Price.HasValue ? game.Price.Value : default,
                Company = game.Company == null ? null : new CompanyDto
                {
                    CompanyId = game.Company.CompanyId,
                    Name = game.Company.Name
                },
                Tags = game.Tags.Select(t => new TagDto
                {
                    TagId = t.TagId,
                    Name = t.Name
                }).ToList(),
                Genres = game.Genres.Select(ge => new GenreDto
                {
                    GenreId = ge.GenreId,
                    Name = ge.Name
                }).ToList()
            };

            return Ok(dto);
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<GameDto>> CreateGame(GameDto dto)
        {
            try
            {
                if (dto == null)
                {
                    Console.WriteLine("DTO is null!");
                    return BadRequest("Game data is required");
                }

                if (string.IsNullOrEmpty(dto.Name))
                {
                    Console.WriteLine("Name is null or empty!");
                    return BadRequest("Game name is required");
                }

                if (dto.CompanyId <= 0)
                {
                    Console.WriteLine($"Invalid CompanyId: {dto.CompanyId}");
                    return BadRequest("Valid CompanyId is required");
                }

                var tags = await _context.Tags.Where(t => dto.TagIds.Contains(t.TagId)).ToListAsync();
                var genres = await _context.Genres.Where(g => dto.GenreIds.Contains(g.GenreId)).ToListAsync();

                var game = new Game
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    ReleaseDate = dto.ReleaseDate,
                    CompanyId = dto.CompanyId,
                    Tags = tags,
                    Genres = genres
                };

                _context.Games.Add(game);
                await _context.SaveChangesAsync();

                await _context.Entry(game).Reference(g => g.Company).LoadAsync();

                var readDto = new GameDto
                {
                    AppId = game.AppId,
                    Name = game.Name,
                    Description = game.Description,
                    ReleaseDate = game.ReleaseDate.HasValue ? game.ReleaseDate.Value : default,
                    Price = game.Price ?? 0,
                    Company = game.Company == null ? null : new CompanyDto
                    {
                        CompanyId = game.Company.CompanyId,
                        Name = game.Company.Name
                    },
                    Tags = tags.Select(t => new TagDto
                    {
                        TagId = t.TagId,
                        Name = t.Name
                    }).ToList(),
                    Genres = genres.Select(ge => new GenreDto
                    {
                        GenreId = ge.GenreId,
                        Name = ge.Name
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetGame), new { id = game.AppId }, readDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateGame: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BadRequest($"Error creating game: {ex.Message}");
            }
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, GameDto dto)
        {
            var existingGame = await _context.Games
                .Include(g => g.Tags)
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(g => g.AppId == id);

            if (existingGame == null)
                return NotFound();

            existingGame.Name = dto.Name;
            existingGame.Description = dto.Description;
            existingGame.Price = dto.Price;
            existingGame.ReleaseDate = dto.ReleaseDate;
            existingGame.CompanyId = dto.CompanyId;

            existingGame.Tags = await _context.Tags.Where(t => dto.TagIds.Contains(t.TagId)).ToListAsync();
            existingGame.Genres = await _context.Genres.Where(g => dto.GenreIds.Contains(g.GenreId)).ToListAsync();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games
                .Include(g => g.Tags)
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(g => g.AppId == id);

            if (game == null)
                return NotFound();

            game.Tags.Clear();
            game.Genres.Clear();
            await _context.SaveChangesAsync();

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
