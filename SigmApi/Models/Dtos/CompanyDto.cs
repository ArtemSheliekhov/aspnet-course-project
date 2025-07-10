using SigmApi.Helpers;

namespace SigmApi.Models.Dtos
{
    public class CompanyDto
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public int? FoundedYear { get; set; }
        public string Headquarters { get; set; }
        public List<WorkerDto> Workers { get; set; } = new();
        public List<GameDto> Games { get; set; } = new();

        public static implicit operator Company(CompanyDto cli)
            => new Company().CopyProperties(cli);
        public static implicit operator CompanyDto(Company cli)
            => new CompanyDto().CopyProperties(cli);
    }

}
