using SigmApi.Helpers;

namespace SigmApi.Models.Dtos
{
    public class WorkerDto
    {
        public int CompanyId { get; set; }
        public int WorkerId { get; set; }
        public string? Position { get; set; }
        public string? Name { get; set; }
        public DateOnly? JoinedDate { get; set; }

        public static implicit operator Worker(WorkerDto cli)
             => new Worker().CopyProperties(cli);
        public static implicit operator WorkerDto(Worker cli)
            => new WorkerDto().CopyProperties(cli);
    }
}
