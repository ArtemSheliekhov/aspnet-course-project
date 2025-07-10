using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SigmApi.Services;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/steam")]
public class UserStatsController : ControllerBase
{
    private readonly IGamesService _steamGamesService;

    public UserStatsController(IGamesService userGamesService)
    {
        _steamGamesService = userGamesService;
    }

    [HttpGet("owned-games")]
    public async Task<IActionResult> GetOwnedGames([FromQuery] string steamId)
    {
        try
        {
            var result = await _steamGamesService.GetSteamUserGamesAsync(steamId);


            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal error: {ex.Message}");
        }
    }
}
