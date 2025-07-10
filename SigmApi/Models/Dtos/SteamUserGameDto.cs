namespace SigmApi.Models.Dtos
{    public class SteamUserGameDto
    {
        public int AppId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Playtime { get; set; }
        public string CapsuleImage { get; set; } = string.Empty;
    }
}
