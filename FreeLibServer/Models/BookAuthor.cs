using System.ComponentModel.DataAnnotations.Schema;

namespace FreeLibServer.Models
{
    [Table("BookAuthors")]
    public class BookAuthor
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
