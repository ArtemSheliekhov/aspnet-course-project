using Microsoft.AspNetCore.Mvc;
using SigmApi.Models;
using SigmApi.Models.Dtos;

namespace SigmApi.Services
{
    public interface IGamesService
    {
        Task<SteamGamesResponseDto> GetTopGamesAsync();
        Task<SteamUserGamesResponseDto> GetSteamUserGamesAsync(string steamId);
        Task<GameDetailDto> GetGameDetailAsync(int id);
    }
}
