using System.Collections.Generic;
using System.Threading.Tasks;
using FreeLibServer.Core.Models;

namespace FreeLibServer.Core
{
    public interface IGenreRepository
    {
        Task<IEnumerable<int>> GetGenreIds();
        Task<IEnumerable<Genre>> GetGenres();
        Task<Genre> GetGenre(int id);
        Task Add(Genre genre);
        void Remove(Genre genre);
    }
}