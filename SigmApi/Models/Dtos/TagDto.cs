using SigmApi.Helpers;

namespace SigmApi.Models.Dtos
{
    public class TagDto
    {
        public int TagId { get; set; }
        public string Name { get; set; }

        public static implicit operator Tag(TagDto cli)
            => new Tag().CopyProperties(cli);
        public static implicit operator TagDto(Tag cli)
            => new TagDto().CopyProperties(cli);
    }
}
