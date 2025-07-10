namespace SigmApi.Models.Dtos
{
    public class SteamGamesResponseDto
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public IEnumerable<SteamGameDto>? Games { get; set; }
    }
}
