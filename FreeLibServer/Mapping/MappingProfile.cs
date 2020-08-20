using AutoMapper;
using FreeLibServer.Controllers.Resources;
using FreeLibServer.Models;
using System.Linq;

namespace FreeLibServer.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Domain to API resources
            CreateMap<Book, BookResource>()
                .ForMember(br => br.Authors, opt => opt.MapFrom(b => b.Authors.Select(ba => ba.AuthorId)))
                .ForMember(br => br.Genres, opt => opt.MapFrom(b => b.Genres.Select(ba => ba.GenreId)));

            //API resources to domain
            CreateMap<BookResource, Book>()
                .ForMember(b => b.Id, opt => opt.Ignore())
                .ForMember(b => b.Authors, opt => opt.Ignore())
                .ForMember(b => b.Genres, opt => opt.Ignore())
                .AfterMap((br, b) => {

                    //удалить авторов и жанры

                    var removedAuthors = b.Authors.Where(a => !br.Authors.Contains(a.AuthorId)).ToList();
                    foreach (var a in removedAuthors)
                        b.Authors.Remove(a);

                    var removedGenres = b.Genres.Where(g => !br.Genres.Contains(g.GenreId)).ToList();
                    foreach (var g in removedGenres)
                        b.Genres.Remove(g);

                    //добавить авторов и жанры

                    var addedAuthors = br.Authors.Where(id => !b.Authors.Any(a => a.AuthorId == id)).Select(id => new BookAuthor { AuthorId = id});
                    foreach (var a in addedAuthors)
                        b.Authors.Add(a);

                    var addedGenres = br.Genres.Where(id => !b.Genres.Any(g => g.GenreId == id)).Select(id => new BookGenre { GenreId = id });

                    foreach (var g in addedGenres)
                        b.Genres.Add(g);

                });

        }
    }
}
