namespace SigmApi.Models.Dtos
{
    public class GameDetailDto
    {
        public int AppId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ShortDescription { get; set; }
        public string DetailedDescription { get; set; }
        public string Website { get; set; }
        public string HeaderImage { get; set; }
        public string CapsuleImage { get; set; }
        public List<string> Developers { get; set; }
        public List<string> Publishers { get; set; }
        public List<string> SupportedLanguages { get; set; }
        public Dictionary<string, string> PcRequirements { get; set; }
        public Dictionary<string, string> MacRequirements { get; set; }
        public Dictionary<string, string> LinuxRequirements { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Genres { get; set; }
        public List<string> Screenshots { get; set; }
        public List<string> Videos { get; set; } = new();
    }
}
