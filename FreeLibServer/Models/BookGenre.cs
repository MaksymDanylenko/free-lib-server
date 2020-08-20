using System.ComponentModel.DataAnnotations.Schema;

namespace FreeLibServer.Models
{
    [Table("BookGenres")]
    public class BookGenre
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
