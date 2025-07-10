using SigmApi.Models;
using SigmApi.Models.Dtos;

namespace SigmApi.Services
{
    public interface ISteamService
    {
        Task<Dictionary<string, object>> LoginWithCredentialsAsync(string username, string password);
        Task<Dictionary<string, object>> LogoutAsync(string sessionId);
    }
}
