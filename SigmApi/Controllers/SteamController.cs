using Microsoft.AspNetCore.Mvc;
using SigmApi.Models.Dtos;
using SigmApi.Services;
using System;
using System.Threading.Tasks;

namespace SigmApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SteamController : ControllerBase
    {
        private readonly ISteamService _steamService;

        public SteamController(ISteamService steamService)
        {
            _steamService = steamService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginSteam([FromBody] LoginRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { error = "Username and password are required" });
            }

            var result = await _steamService.LoginWithCredentialsAsync(
                request.Username,
                request.Password
            );

            if (result.ContainsKey("success") && (bool)result["success"])
            {
                return Ok(new
                {
                    success = true,
                    sessionId = result["sessionId"],
                    username = result["username"]
                });
            }

            return BadRequest(result);
        }


        [HttpPost("logout/{sessionId}")]
        public async Task<IActionResult> Logout(string sessionId)
        {
            var result = await _steamService.LogoutAsync(sessionId);
            return Ok(result);
        }
    }
}