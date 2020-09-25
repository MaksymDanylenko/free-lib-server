using System.Collections.Generic;
using System.Threading.Tasks;
using FreeLibServer.Core.Models;

namespace FreeLibServer.Core
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<int>> GetAuthorIds();
        Task<IEnumerable<Author>> GetAuthors();
        Task<Author> GetAuthor(int id, bool includeRelated = true);
        Task Add(Author author);
        void Remove(Author author);
    }
}