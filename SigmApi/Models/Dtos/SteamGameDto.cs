namespace SigmApi.Models.Dtos
{
    public class SteamGameDto
    {
        public int AppId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CurrentPlayers { get; set; }
        public int Rank { get; set; }
        public int PeakInGame { get; set; }
        public string CapsuleImage { get; set; } = string.Empty;
    }
}
