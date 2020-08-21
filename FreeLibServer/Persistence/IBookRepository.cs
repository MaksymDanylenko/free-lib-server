using System.Threading.Tasks;
using FreeLibServer.Models;

namespace FreeLibServer.Persistence
{
    public interface IBookRepository
    {
         Task<Book> GetBook(int id, bool includeRelated = true);
         void Add(Book book);
         void Remove(Book book);
    }
}