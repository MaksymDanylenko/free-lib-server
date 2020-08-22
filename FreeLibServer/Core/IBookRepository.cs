using System.Collections.Generic;
using System.Threading.Tasks;
using FreeLibServer.Core.Models;

namespace FreeLibServer.Core
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooks();
        Task<Book> GetBook(int id, bool includeRelated = true);
        Task Add(Book book);
        void Remove(Book book);
    }
}