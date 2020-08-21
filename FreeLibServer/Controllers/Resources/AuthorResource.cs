using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using FreeLibServer.Models;

namespace FreeLibServer.Controllers.Resources
{
    public class AuthorResource : KeyValuePairResource
    {

        public string Info { get; set; }

        public ICollection<int> Books { get; set; }

        public AuthorResource()
        {
            Books = new Collection<int>();
        }
    }
}