using AutoMapper;
using FreeLibServer.Controllers.Resources;
using FreeLibServer.Models;
using FreeLibServer.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FreeLibServer.Controllers
{
    [Route("/api/books")]
    public class BooksController : Controller {
        private readonly FreeLibDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(FreeLibDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateBook([FromBody] BookResource bookResource) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var book = _mapper.Map<BookResource, Book>(bookResource);

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<Book, BookResource>(book);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookResource bookResource) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var book = await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .SingleOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound();

            _mapper.Map<BookResource, Book>(bookResource, book);

            await _context.SaveChangesAsync();

            var result = _mapper.Map<Book, BookResource>(book);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id) {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return NotFound();

            _context.Remove(book);
            await _context.SaveChangesAsync();
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id) {
            var book = await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .SingleOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound();

            return Ok(_mapper.Map<Book, BookResource>(book));
        }

    }
}
