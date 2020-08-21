using System.Threading.Tasks;
using FreeLibServer.Core.Models;

namespace FreeLibServer.Core
{
    public interface IBookRepository
    {
         Task<Book> GetBook(int id, bool includeRelated = true);
         void Add(Book book);
         void Remove(Book book);
    }
}