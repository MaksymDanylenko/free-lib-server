using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeLibServer.Core;
using FreeLibServer.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FreeLibServer.Persistence
{
    public class AuthorRepository : IAuthorRepository
    {
        FreeLibDbContext _context;
        public AuthorRepository(FreeLibDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Author>> GetAuthors() {
            return await _context.Authors.ToListAsync();
        }
        public async Task<Author> GetAuthor(int id, bool includeRelated = true) {
            if (!includeRelated)
                return await _context.Authors.FindAsync(id);

            return await _context.Authors
                .Include(a => a.Books)
                .ThenInclude(ba => ba.Book)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task Add(Author author) {
            await _context.Authors.AddAsync(author);
        }

        public void Remove(Author author) {
            _context.Remove(author);
        }

        public async Task<IEnumerable<int>> GetAuthorIds()
        {
            return await _context.Authors.Select(a => a.Id).ToArrayAsync();
        }
    }
}