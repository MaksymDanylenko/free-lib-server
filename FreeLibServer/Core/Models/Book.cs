using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreeLibServer.Core.Models
{
    [Table("Books")]
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<BookAuthor> Authors { get; set; }

        public ICollection<BookGenre> Genres { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string Info { get; set; }

        public int Year { get; set; }

        public int NumberOfPages { get; set; }

        public string Contents { get; set; }

        public Book()
        {
            Authors = new Collection<BookAuthor>();
            Genres = new Collection<BookGenre>();
        }
    }
}
