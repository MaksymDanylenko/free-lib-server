using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FreeLibServer.Controllers.Resources
{
    public class BookResource : KeyValuePairResource
    {

        public ICollection<KeyValuePairResource> Authors { get; set; }

        public ICollection<KeyValuePairResource> Genres { get; set; }

        [Required]
        public string Category { get; set; }

        public string Info { get; set; }

        public int Year { get; set; }

        public int NumberOfPages { get; set; }

        public string Contents { get; set; }

        public BookResource()
        {
            Authors = new Collection<KeyValuePairResource>();
            Genres = new Collection<KeyValuePairResource>();
        }
    }
}