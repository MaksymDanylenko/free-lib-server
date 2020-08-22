using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FreeLibServer.Controllers.Resources;
using FreeLibServer.Core;
using FreeLibServer.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreeLibServer.Controllers
{
    [Route("/api/authors/")]
    public class AuthorsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AuthorsController(IMapper mapper, IAuthorRepository repository, IUnitOfWork unitOfWork) {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAuthor([FromBody] SaveAuthorResource authorResource) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var author = _mapper.Map<SaveAuthorResource, Author>(authorResource);

            await _repository.Add(author);
            await _unitOfWork.CompleteAsync();

            author = await _repository.GetAuthor(author.Id);

            var result = _mapper.Map<Author, SaveAuthorResource>(author);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] SaveAuthorResource authorResource) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var author = await _repository.GetAuthor(id);

            if (author == null)
                return NotFound();

            _mapper.Map<SaveAuthorResource, Author>(authorResource, author);

            await _unitOfWork.CompleteAsync();

            author = await _repository.GetAuthor(author.Id);
            var result = _mapper.Map<Author, SaveAuthorResource>(author);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id) {
            var author = await _repository.GetAuthor(id, includeRelated: false);

            if (author == null)
                return NotFound();

            _repository.Remove(author);
            await _unitOfWork.CompleteAsync();
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id) {
            var author = await _repository.GetAuthor(id);

            if (author == null)
                return NotFound();

            return Ok(_mapper.Map<Author, AuthorResource>(author));
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors() {
            var authors = await _repository.GetAuthors();
            var result = authors.Select(a => _mapper.Map<Author, KeyValuePairResource>(a));
            return Ok(result);
        }
    }
}