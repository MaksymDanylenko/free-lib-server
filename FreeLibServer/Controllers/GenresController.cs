using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FreeLibServer.Controllers.Resources;
using FreeLibServer.Core;
using FreeLibServer.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FreeLibServer.Controllers
{
    [Route("/api/genres")]
    public class GenresController : Controller
    {
        private readonly IGenreRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GenresController(IGenreRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetGenres() {
            var genres = await _repository.GetGenres();
            var result = genres.Select(g => _mapper.Map<Genre, KeyValuePairResource>(g));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenre(int id) {
            var genre = await _repository.GetGenre(id);

            if (genre == null) {
                return NotFound();
            }

            return Ok(_mapper.Map<Genre, KeyValuePairResource>(genre));
        }

        [HttpPost]
        public async Task<IActionResult> AddGenre([FromBody] KeyValuePairResource genreResource) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var genre = _mapper.Map<KeyValuePairResource, Genre>(genreResource);
            await _repository.Add(genre);
            await _unitOfWork.CompleteAsync();

            genre = await _repository.GetGenre(genre.Id);
            return Ok(_mapper.Map<Genre, KeyValuePairResource>(genre));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id) {
            var genre = await _repository.GetGenre(id);

            if (genre == null)
                return NotFound();

            _repository.Remove(genre);
            await _unitOfWork.CompleteAsync();

            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] KeyValuePairResource genreResource) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var genre = await _repository.GetGenre(id);

            if (genre == null)
                return NotFound();

            _mapper.Map<KeyValuePairResource, Genre>(genreResource, genre);
            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<Genre, KeyValuePairResource>(genre));
        }
    }
}