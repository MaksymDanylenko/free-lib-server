using System.Collections.Generic;
using System.Threading.Tasks;
using FreeLibServer.Core;
using FreeLibServer.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FreeLibServer.Persistence
{
    public class BookRepository : IBookRepository
    {
        FreeLibDbContext _context;
        public BookRepository(FreeLibDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Book>> GetBooks() {
            return await _context.Books
                .Include(b => b.Authors)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.Genres)
                .ThenInclude(bg => bg.Genre)
                .ToListAsync();
        }
        public async Task<Book> GetBook(int id, bool includeRelated = true) {
            if (!includeRelated)
                return await _context.Books.FindAsync(id);

            return await _context.Books
                .Include(b => b.Authors)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.Genres)
                .ThenInclude(bg => bg.Genre)
                .SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task Add(Book book) {
            await _context.Books.AddAsync(book);
        }

        public void Remove(Book book) {
            _context.Remove(book);
        }
    }
}