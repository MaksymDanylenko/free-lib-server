using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FreeLibServer.Controllers.Resources
{
    public class GenreResource : KeyValuePairResource
    {

        public ICollection<int> Books { get; set; }

        public GenreResource()
        {
            Books = new Collection<int>();
        }
    }
}