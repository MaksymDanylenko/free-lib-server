using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeLibServer.Controllers.Parameters;
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
        /*public async Task<IEnumerable<Book>> GetBooks() {
            return await _context.Books
                .Include(b => b.Authors)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.Genres)
                .ThenInclude(bg => bg.Genre)
                .ToListAsync();
        }*/

        public async Task<IEnumerable<Book>> GetBooks(BookParameters bookParameters) {
            var books = await _context.Books
                .Where(b =>
                    b.Year >= bookParameters.MinYear
                    && b.Year <= bookParameters.MaxYear)
                .Include(b => b.Authors)
                .ThenInclude(ba => ba.Author)
                //.Where(b => 
                //    Enumerable.Intersect(bookParameters.Authors, b.Authors.Select(a => a.AuthorId)).Count() > 0)
                .Include(b => b.Genres)
                .ThenInclude(bg => bg.Genre)
                //.Where(b => 
                //    Enumerable.Intersect(bookParameters.Genres, b.Genres.Select(g => g.GenreId)).Count() > 0)
                .ToListAsync();
            
            return books
                .Where(b => 
                    Enumerable.Intersect(bookParameters.Authors, b.Authors.Select(a => a.AuthorId)).Count() > 0)
                .Where(b => 
                    Enumerable.Intersect(bookParameters.Genres, b.Genres.Select(g => g.GenreId)).Count() > 0)
                .Skip((bookParameters.PageNumber - 1) * bookParameters.PageSize)
                .Take(bookParameters.PageSize);
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