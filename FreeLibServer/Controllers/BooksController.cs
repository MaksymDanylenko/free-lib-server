using AutoMapper;
using FreeLibServer.Controllers.Resources;
using FreeLibServer.Core.Models;
using FreeLibServer.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using FreeLibServer.Controllers.Parameters;
using Newtonsoft.Json;

namespace FreeLibServer.Controllers
{
    [Route("/api/books")]
    public class BooksController : Controller {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(IMapper mapper,
                IBookRepository bookRepository,
                IAuthorRepository authorRepository,
                IGenreRepository genreRepository,
                IUnitOfWork unitOfWork) {
            _mapper = mapper;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateBook([FromBody] SaveBookResource bookResource) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var book = _mapper.Map<SaveBookResource, Book>(bookResource);

            await _bookRepository.Add(book);
            await _unitOfWork.CompleteAsync();

            book = await _bookRepository.GetBook(book.Id);

            var result = _mapper.Map<Book, BookResource>(book);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] SaveBookResource bookResource) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var book = await _bookRepository.GetBook(id);

            if (book == null)
                return NotFound();

            _mapper.Map<SaveBookResource, Book>(bookResource, book);

            await _unitOfWork.CompleteAsync();

            book = await _bookRepository.GetBook(book.Id);
            var result = _mapper.Map<Book, BookResource>(book);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id) {
            var book = await _bookRepository.GetBook(id, includeRelated: false);

            if (book == null)
                return NotFound();

            _bookRepository.Remove(book);
            await _unitOfWork.CompleteAsync();
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id) {
            var book = await _bookRepository.GetBook(id);

            if (book == null)
                return NotFound();

            return Ok(_mapper.Map<Book, BookResource>(book));
        }

        /*[HttpGet]
        public async Task<IActionResult> GetBooks() {
            var books = await _repository.GetBooks();
            var result = books.Select(b => _mapper.Map<Book, BookResource>(b));
            return Ok(result);
        }*/

        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] BookParameters bookParameters) {

            if (bookParameters.Authors == null)
            {
                bookParameters.Authors = await _authorRepository.GetAuthorIds() as int[];
            }

            if (bookParameters.Genres == null)
            {
                bookParameters.Genres = await _genreRepository.GetGenreIds() as int[];
            }

            var books = await _bookRepository.GetBooks(bookParameters);
            var result = books.Select(b => _mapper.Map<Book, BookResource>(b));
            return Ok(result);
        }

    }
}
