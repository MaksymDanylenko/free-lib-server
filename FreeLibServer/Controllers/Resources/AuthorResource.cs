using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FreeLibServer.Controllers.Resources
{
    public class AuthorResource : SaveAuthorResource
    {
        public ICollection<int> Books { get; set; }

        public AuthorResource()
        {
            Books = new Collection<int>();
        }
    }
}