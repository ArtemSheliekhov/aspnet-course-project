using Newtonsoft.Json.Linq;
using SigmApi.Models.Dtos;

namespace SigmApi.Services
{
    public class GamesService : IGamesService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private const string SteamCdnUrl = "https://cdn.akamai.steamstatic.com/steam/apps/";

        public GamesService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }


        public async Task<SteamGamesResponseDto> GetTopGamesAsync()
        {
            try
            {
                var SteamApiKey = _configuration["SteamApiKey"];
                var gamesListUrl = $"https://api.steampowered.com/ISteamChartsService/GetGamesByConcurrentPlayers/v1/?key={SteamApiKey}";
                var response = await _httpClient.GetAsync(gamesListUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var responseData = JObject.Parse(content);

                var games = await Task.WhenAll(
                    responseData["response"]?["ranks"] != null  
                    ? responseData["response"]["ranks"]
                        .Select(async rank =>
                        {
                            var appId = rank["appid"]?.Value<int>() ?? 0;
                            var (name, capsuleImage) = await GetGameInfoAsync(appId);

                            return new SteamGameDto
                            {
                                AppId = appId,
                                Name = name,
                                CurrentPlayers = rank["concurrent_in_game"]?.Value<int>() ?? 0,
                                Rank = rank["rank"]?.Value<int>() ?? 0, 
                                PeakInGame = rank["peak_in_game"]?.Value<int>() ?? 0,
                                CapsuleImage = capsuleImage
                            };
                        })
                    : Enumerable.Empty<Task<SteamGameDto>>());

                return new SteamGamesResponseDto
                {
                    Success = true,
                    Games = games
                };
            }
            catch (Exception ex)
            {
                return new SteamGamesResponseDto
                {
                    Success = false,
                    ErrorMessage = $"Failed to retrieve games: {ex.Message}"
                };
            }
        }


        private async Task<(string Name, string CapsuleImage)> GetGameInfoAsync(int appId)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"https://store.steampowered.com/api/appdetails?appids={appId}&filters=basic&l=english");

                var content = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(content);

                return (
                    Name: data[appId.ToString()]?["data"]?["name"]?.ToString() ?? "Unknown",
                    CapsuleImage: data[appId.ToString()]?["data"]?["capsule_image"]?.ToString()
                                  ?? $"{SteamCdnUrl}{appId}/capsule_231x87.jpg"
                );
            }
            catch
            {
                return ("Unknown", $"{SteamCdnUrl}{appId}/capsule_231x87.jpg");
            }
        }

        public async Task<SteamUserGamesResponseDto> GetSteamUserGamesAsync(string steamId)
        {

            try
            {
                var SteamApiKey = _configuration["SteamApiKey"];
                var ownedGamesUrl = $"https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/" +
                               $"?steamid={steamId}" +
                               $"&key={SteamApiKey}" +
                               $"&include_appinfo=true" +
                               $"&include_played_free_games=true";

                var ownedGamesResponse = await _httpClient.GetAsync(ownedGamesUrl);
                ownedGamesResponse.EnsureSuccessStatusCode();

                var ownedGamesContent = await ownedGamesResponse.Content.ReadAsStringAsync();
                var ownedGamesData = JObject.Parse(ownedGamesContent);

                var games = ownedGamesData["response"]?["games"]?
                    .Select(g => new SteamUserGameDto
                    {
                        AppId = (int)g["appid"],
                        Name = (string)g["name"] ?? "Unknown",
                        Playtime = (int)g["playtime_forever"],
                        CapsuleImage = $"{SteamCdnUrl}{(int)g["appid"]}/capsule_184x69.jpg",
                    })
                    .OrderByDescending(g => g.Playtime)
                    .ToList();


                return new SteamUserGamesResponseDto
                {
                    Success = true,
                    Games = games
                };
            }
            catch (Exception ex)
            {
                return new SteamUserGamesResponseDto
                {
                    Success = false,
                    ErrorMessage = $"Failed to retrieve games: {ex.Message}"
                };
            }
        }

        public async Task<GameDetailDto> GetGameDetailAsync(int appId)
        {
            try
            {
                var url = $"https://store.steampowered.com/api/appdetails?appids={appId}&l=en";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);
                var data = json[appId.ToString()]?["data"];

                if (data == null) return null;

                return new GameDetailDto
                {
                    AppId = appId,
                    Name = data["name"]?.ToString() ?? "Unknown",
                    Type = data["type"]?.ToString(),
                    ShortDescription = data["short_description"]?.ToString(),
                    DetailedDescription = data["detailed_description"]?.ToString(),
                    Website = data["website"]?.ToString(),
                    HeaderImage = data["header_image"]?.ToString(),
                    CapsuleImage = data["capsule_image"]?.ToString() ?? $"{SteamCdnUrl}{appId}/capsule_231x87.jpg",
                    Developers = data["developers"]?.Select(d => d.ToString()).ToList() ?? new(),
                    Publishers = data["publishers"]?.Select(p => p.ToString()).ToList() ?? new(),
                    SupportedLanguages = data["supported_languages"]?.ToString()
                                         ?.Split(',')
                                         ?.Select(l => l.Trim().Split('<')[0])
                                         ?.ToList() ?? new(),
                    PcRequirements = ParseRequirements(data["pc_requirements"]),
                    Categories = data["categories"]?.Select(c => c["description"]?.ToString()).ToList() ?? new(),
                    Genres = data["genres"]?.Select(g => g["description"]?.ToString()).ToList() ?? new(),
                    Screenshots = data["screenshots"]?.Select(s => s["path_full"]?.ToString()).ToList() ?? new(),
                    Videos = data["movies"]
                        ?.Select(m =>
                            (m["webm"]?["max"]?.ToString() ?? m["mp4"]?["max"]?.ToString())
                            ?.Replace("http://", "https://"))
                        .Where(v => !string.IsNullOrEmpty(v))
                        .ToList() ?? new()
                };
            }
            catch
            {
                return null;
            }
        }

        private Dictionary<string, string> ParseRequirements(JToken? reqToken)
        {
            var dict = new Dictionary<string, string>();
            if (reqToken?["minimum"] != null)
            {
                dict["Minimum"] = reqToken["minimum"].ToString();
            }
            if (reqToken?["recommended"] != null)
            {
                dict["Recommended"] = reqToken["recommended"].ToString();
            }
            return dict;
        }


        #region mapping

        private SteamGameDto MapToGameDto(JProperty property)
        {
            var gameData = (JObject)property.Value;
            var appId = int.Parse(property.Name);

            return new SteamGameDto
            {
                AppId = appId,
                Name = gameData["name"]?.ToString() ?? "Unknown",
                CurrentPlayers = gameData["ccu"]?.ToObject<int>() ?? 0,
                //PositiveRating = gameData["positive"]?.ToObject<int>() ?? 0,
                CapsuleImage = $"{SteamCdnUrl}{appId}/capsule_184x69.jpg"
            };
        }

        /*        private SteamUserGameDto MapToUserGameDto(JProperty property)
                {
                    var gameData = (JObject)property.Value;
                    var appId = int.Parse(property.Name);

                    return new SteamUserGameDto
                    {
                        AppId = appId,
                        Name = gameData["name"]?.ToString() ?? "Unknown",
                        Playtime = gameData["playtime_forever"]?.ToObject<int>() ?? 0,
                        CapsuleImage = $"{SteamCdnUrl}{appId}/capsule_184x69.jpg"
                    };
                }*/
        #endregion

    }
}
