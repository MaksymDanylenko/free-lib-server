using System.Collections.Generic;
using System.Threading.Tasks;
using FreeLibServer.Core;
using FreeLibServer.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FreeLibServer.Persistence
{
    public class GenreRepository : IGenreRepository
    {
        private readonly FreeLibDbContext _context;

        public GenreRepository(FreeLibDbContext context)
        {
            _context = context;
        }
        public async Task Add(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
        }

        public async Task<Genre> GetGenre(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            return await _context.Genres.ToListAsync();
        }

        public void Remove(Genre genre)
        {
            _context.Remove(genre);
        }
    }
}