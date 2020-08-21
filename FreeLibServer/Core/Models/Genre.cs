using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreeLibServer.Core.Models
{
    [Table("Genres")]
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<BookGenre> Books { get; set; }

        public Genre()
        {
            Books = new Collection<BookGenre>();
        }
    }
}
