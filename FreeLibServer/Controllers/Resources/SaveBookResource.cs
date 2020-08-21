using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FreeLibServer.Controllers.Resources
{
    public class SaveBookResource
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<int> Authors { get; set; }

        public ICollection<int> Genres { get; set; }

        public int CategoryId { get; set; }

        public string Info { get; set; }

        public int Year { get; set; }

        public int NumberOfPages { get; set; }

        public string Contents { get; set; }

        public SaveBookResource()
        {
            Authors = new Collection<int>();
            Genres = new Collection<int>();
        }
    }
}
