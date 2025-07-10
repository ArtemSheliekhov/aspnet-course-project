using Microsoft.AspNetCore.Mvc;
using SigmApi.Models.Dtos;
using SigmApi.Services;

namespace SigmApi.Controllers
{
    [ApiController]
    [Route("api/steam")]
    public class SteamGamesController : ControllerBase
    {
        private readonly IGamesService _steamGamesService;

        public SteamGamesController(IGamesService steamGamesService)
        {
            _steamGamesService = steamGamesService;
        }

        [HttpGet("top-games")]
        [ProducesResponseType(typeof(SteamGamesResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopGames()
        {
            var result = await _steamGamesService.GetTopGamesAsync();


            return Ok(result);
        }

        [HttpGet("game-details/{appId}")]
        public async Task<IActionResult> GetGameDetails(int appId)
        {
            var game = await _steamGamesService.GetGameDetailAsync(appId);
            return Ok(game);
        }



    }
}