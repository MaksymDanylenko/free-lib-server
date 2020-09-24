using AutoMapper;
using FreeLibServer.Controllers.Resources;
using FreeLibServer.Core.Models;
using FreeLibServer.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using FreeLibServer.Controllers.Parameters;

namespace FreeLibServer.Controllers
{
    [Route("/api/books")]
    public class BooksController : Controller {
        private readonly IMapper _mapper;
        private readonly IBookRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(IMapper mapper, IBookRepository repository, IUnitOfWork unitOfWork) {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateBook([FromBody] SaveBookResource bookResource) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var book = _mapper.Map<SaveBookResource, Book>(bookResource);

            await _repository.Add(book);
            await _unitOfWork.CompleteAsync();

            book = await _repository.GetBook(book.Id);

            var result = _mapper.Map<Book, BookResource>(book);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] SaveBookResource bookResource) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var book = await _repository.GetBook(id);

            if (book == null)
                return NotFound();

            _mapper.Map<SaveBookResource, Book>(bookResource, book);

            await _unitOfWork.CompleteAsync();

            book = await _repository.GetBook(book.Id);
            var result = _mapper.Map<Book, BookResource>(book);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id) {
            var book = await _repository.GetBook(id, includeRelated: false);

            if (book == null)
                return NotFound();

            _repository.Remove(book);
            await _unitOfWork.CompleteAsync();
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id) {
            var book = await _repository.GetBook(id);

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
            var books = await _repository.GetBooks(bookParameters);
            var result = books.Select(b => _mapper.Map<Book, BookResource>(b));
            return Ok(result);
        }

    }
}
