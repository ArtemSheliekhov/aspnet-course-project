using SigmApi.Helpers;

namespace SigmApi.Models.Dtos
{
    public class GameDto
    {
        public int AppId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public int CompanyId { get; set; }
        public List<int> TagIds { get; set; } = new();
        public List<int> GenreIds { get; set; } = new();
        public CompanyDto? Company { get; set; }
        public List<TagDto>? Tags { get; set; }
        public List<GenreDto>? Genres { get; set; }

        public static implicit operator Game(GameDto cli)
           => new Game().CopyProperties(cli);
        public static implicit operator GameDto(Game cli)
            => new GameDto().CopyProperties(cli);
    }
}
