using SigmApi.Helpers;
using SigmApi.Models;

namespace SigmApi.Models.Dtos
{
    public class GenreDto
    {
        public int GenreId { get; set; }
        public string Name { get; set; }

        public static implicit operator Genre(GenreDto cli)
            => new Genre().CopyProperties(cli);
        public static implicit operator GenreDto(Genre cli)
            => new GenreDto().CopyProperties(cli);
    }
}


