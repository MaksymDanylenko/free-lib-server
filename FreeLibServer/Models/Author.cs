using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreeLibServer.Models
{
    [Table("Authors")]
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Info { get; set; }

        public ICollection<BookAuthor> Books { get; set; }

        public Author()
        {
            Books = new Collection<BookAuthor>();
        }
    }
}
