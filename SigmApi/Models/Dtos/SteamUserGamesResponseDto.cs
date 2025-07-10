namespace SigmApi.Models.Dtos
{
    public class SteamUserGamesResponseDto
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public IEnumerable<SteamUserGameDto>? Games { get; set; }
    }
}
